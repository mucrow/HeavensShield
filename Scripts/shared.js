const fs = require('fs/promises');
const path = require('path');

exports.BUILDS_DIR = path.join('.', 'Builds');

exports.buildDirNameInvalidExit = function() {
  console.error('Build directory name must be a semantic version number prefixed with a \'v\', such as "v2.14.3"');
  process.exit(1);
}

exports.buildsDirNotFoundExit = function() {
  console.error(`Couldn't access ${path.resolve(BUILDS_DIR)}`);
  console.error('\nBuilds directory not found (or insufficient permissions).');
  console.error('Be sure to run this script in the root directory of the MobileTowerDefense repository.');
  process.exit(1);
}

exports.isBuildDirNameValid = function(name) {
  return /^v\d+\.\d+\.\d+$/.test(name);
}

exports.isDirEmpty = async function(path) {
  try {
    const files = await fs.readdir(path);
    return files.length <= 0;
  } catch (err) {
    console.error(err);
  }
}

exports.dirExists = async function(path) {
  try {
    await fs.access(path);
    return true;
  }
  catch (e) {
    //
  }
  return false;
}
