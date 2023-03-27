const fs = require('fs/promises');
const path = require('path');

const archiver = require('archiver');

exports.zip = async function(contentsPath, isDir) {
  if (typeof isDir === 'undefined') {
    throw new Error('You must provide an argument for isDir.');
  }

  return new Promise(async (resolve, reject) => {
    // create a file to stream archive data to.
    const outputFile = await fs.open(`${contentsPath}.zip`, 'w');
    const output = outputFile.createWriteStream();
    const archive = archiver('zip', { zlib: { level: 9 } });

    output.on('close', function() {
      resolve();
    });

    archive.on('error', function(err) {
      reject(err);
    });

    archive.pipe(output);

    if (isDir) {
      // append files from a sub-directory, putting its contents at the root of archive
      archive.directory(contentsPath, path.basename(contentsPath));
    }
    else {
      archive.file(contentsPath, { name: path.basename(contentsPath) });
    }

    // append files from a sub-directory and naming it `new-subdir` within the archive
    // archive.directory('subdir/', 'new-subdir');

    archive.finalize();
  });
}
