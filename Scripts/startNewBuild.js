const fs = require('fs/promises');
const path = require('path');

const { getInfoForPlatforms } = require('./platformInfo');
const {
  BUILDS_DIR,
  buildDirNameInvalidExit,
  buildsDirNotFoundExit,
  isBuildDirNameValid,
  dirExists
} = require('./shared');

async function main(args) {
  if (args.length !== 3) {
    const scriptName = path.basename(args[1]);
    console.error(`Usage:\nnode ${scriptName} <build directory name>`);
    process.exit(1);
  }
  const buildDirBaseName = args[2];
  if (!isBuildDirNameValid(buildDirBaseName)) {
    buildDirNameInvalidExit();
  }

  if (!(await dirExists(BUILDS_DIR))) {
    buildsDirNotFoundExit();
  }

  const buildDir = path.join(BUILDS_DIR, buildDirBaseName);
  try {
    await fs.mkdir(buildDir);
  }
  catch (e) {
    console.error(`Couldn't create directory ${path.resolve(buildDir)}`);
    console.error('Please ensure the directory does not exist.');
    process.exit(1);
  }

  const platformsInfo = getInfoForPlatforms(buildDirBaseName, buildDir);
  const dirsToCreate = [
    platformsInfo.win.dir,
    platformsInfo.macos.dir,
    platformsInfo.linux.dir,
    platformsInfo.ios.dir,
    platformsInfo.ios.artifactPath,
    platformsInfo.android.dir
  ];

  for (let i = 0; i < dirsToCreate.length; ++i) {
    const dir = dirsToCreate[i];
    await fs.mkdir(dir);
  }

  console.log('Done.');
}

main(process.argv);
