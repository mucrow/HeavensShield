const fs = require('fs/promises');
const path = require('path');

const BUILDS_DIR = path.join('.', 'Builds');

async function main(args) {
  if (args.length !== 3) {
    const scriptName = path.basename(args[1]);
    console.error(`Usage:\nnode ${scriptName} <build directory name>`);
    process.exit(1);
  }
  const buildDirBaseName = args[2];
  if (!/^v\d+\.\d+\.\d+$/.test(buildDirBaseName)) {
    console.error('Build directory name must be a semantic version number prefixed with a \'v\', such as "v2.14.3"');
    process.exit(1);
  }
  const buildDir = path.join(BUILDS_DIR, buildDirBaseName);

  // throws an error if BUILDS_DIR does not exist or we don't have permission
  // to access it
  try {
    await fs.access(BUILDS_DIR);
  }
  catch (e) {
    console.error(`Couldn't access ${path.resolve(BUILDS_DIR)}`);
    console.error('\nBuilds directory not found (or insufficient permissions).');
    console.error('Be sure to run this script in the root directory of the MobileTowerDefense repository.');
    process.exit(1);
  }

  try {
    await fs.mkdir(buildDir);
  }
  catch (e) {
    console.error(`Couldn't create directory ${path.resolve(buildDir)}`);
    console.error('Please ensure the directory does not exist.');
    process.exit(1);
  }

  const xcodeDir = path.join(buildDir, 'Xcode');
  const dirsToCreate = [
    path.join(buildDir, 'Windows'),
    path.join(buildDir, 'MacOS'),
    path.join(buildDir, 'Linux'),
    xcodeDir,
    path.join(xcodeDir, `MobileTowerDefense-Xcode-${buildDirBaseName}`),
    path.join(buildDir, 'Android')
  ];

  for (let i = 0; i < dirsToCreate.length; ++i) {
    const dir = dirsToCreate[i];
    await fs.mkdir(dir);
  }

  console.log('Done.');
}

main(process.argv);
