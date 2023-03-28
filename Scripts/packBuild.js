const fs = require('fs/promises');
const path = require('path');

const {
  GAME_NAME,
  PLATFORM_CODES,
  getInfoForPlatforms
} = require('./platformInfo');
const {
  BUILDS_DIR,
  dirExists,
  isDirEmpty
} = require('./shared');
const { zip } = require('./archiverAdapter');

async function removeNoShipSubdirs(dir) {
  const subdirA = path.join(dir, `${GAME_NAME}_BackUpThisFolder_ButDontShipItWithYourGame`);
  const subdirB = path.join(dir, `${GAME_NAME}_BurstDebugInformation_DoNotShip`);
  await Promise.all([
    fs.rm(subdirA, { recursive: true }),
    fs.rm(subdirB, { recursive: true })
  ]);
}

async function shouldPackDirectory(path) {
  if (!(await dirExists(path))) {
    return false;
  }
  if (await isDirEmpty(path)) {
    return false;
  }
  return true;
}

function logPreparingMessage(artifact) {
  console.log(`Preparing ${artifact} for packing...`);
}

function logCompressingMessage(artifact) {
  console.log(`Compressing ${artifact}...`);
}

function logCleanupMessage(artifact) {
  console.log(`Cleaning up ${artifact} files we no longer need...`);
}

function logFinishedMessage(artifact) {
  console.log(`${artifact} done.`);
}

async function packWindows(platformsInfo) {
  if (!(await shouldPackDirectory(platformsInfo.win.dir))) {
    return false;
  }

  const info = platformsInfo.win;
  const artifactName = 'Windows build';

  logPreparingMessage(artifactName);
  await fs.cp(info.dir, info.packPath, { recursive: true });
  await removeNoShipSubdirs(info.packPath);

  logCompressingMessage(artifactName);
  await zip(info.packPath, true);

  logCleanupMessage(artifactName);
  await fs.rm(info.packPath, { recursive: true });

  logFinishedMessage(artifactName);
  return true;
}

async function packMacOS(platformsInfo) {
  if (!(await shouldPackDirectory(platformsInfo.macos.dir))) {
    return false;
  }

  const info = platformsInfo.macos;
  const artifactName = 'MacOS build';

  logPreparingMessage(artifactName);
  await fs.cp(info.artifactPath, info.packPath, { recursive: true });

  logCompressingMessage(artifactName);
  await zip(info.packPath, true);

  logCleanupMessage(artifactName);
  await fs.rm(info.packPath, { recursive: true });

  logFinishedMessage(artifactName);
  return true;
}

async function packLinux(platformsInfo) {
  if (!(await shouldPackDirectory(platformsInfo.linux.dir))) {
    return false;
  }

  const info = platformsInfo.linux;
  const artifactName = 'Linux build';

  logPreparingMessage(artifactName);
  await fs.cp(info.dir, info.packPath, { recursive: true });
  await removeNoShipSubdirs(info.packPath);

  logCompressingMessage(artifactName);
  await zip(info.packPath, true);

  logCleanupMessage(artifactName);
  await fs.rm(info.packPath, { recursive: true });

  logFinishedMessage(artifactName);
  return true;
}

async function packIOS(platformsInfo) {
  if (!(await dirExists(platformsInfo.ios.dir))) {
    return false;
  }
  if (!(await shouldPackDirectory(platformsInfo.ios.artifactPath))) {
    return false;
  }

  const info = platformsInfo.ios;
  const artifactName = 'Xcode project';

  logPreparingMessage(artifactName);
  await fs.cp(info.artifactPath, info.packPath, { recursive: true });

  logCompressingMessage(artifactName);
  await zip(info.packPath, true);

  logCleanupMessage(artifactName);
  await fs.rm(info.packPath, { recursive: true });

  logFinishedMessage(artifactName);
  return true;
}

async function packAndroid(platformsInfo) {
  if (!(await shouldPackDirectory(platformsInfo.android.dir))) {
    return false;
  }

  const info = platformsInfo.android;
  const artifactName = 'Android build';

  logPreparingMessage(artifactName);
  await fs.copyFile(info.artifactPath, info.packPath);

  logCompressingMessage(artifactName);
  await zip(info.packPath, false);

  logCleanupMessage(artifactName);
  await fs.rm(info.packPath);

  logFinishedMessage(artifactName);
  return true;
}

const PLATFORM_CODE_TO_PACK_FN = {
  win: packWindows,
  macos: packMacOS,
  linux: packLinux,
  ios: packIOS,
  android: packAndroid
};

async function main(args) {
  if (args.length !== 3) {
    const scriptName = path.basename(args[1]);
    console.error(`Usage:\nnode ${scriptName} <build directory name>`);
    process.exit(1);
  }

  const buildDirBaseName = args[2];
  const buildDir = path.join(BUILDS_DIR, buildDirBaseName);
  if (!(await dirExists(buildDir))) {
    console.error(`Couldn't access ${path.resolve(buildDir)}`);
    console.error(`\nDirectory for build ${buildDirBaseName} not found (or insufficient permissions).`);
    console.error('Be sure to run this script in the root directory of the MobileTowerDefense repository.');
    process.exit(1);
  }
  const platformsInfo = getInfoForPlatforms(buildDirBaseName, buildDir);

  const promises = [];
  for (let i = 0; i < PLATFORM_CODES.length; ++i) {
    const platform = PLATFORM_CODES[i];
    const packFn = PLATFORM_CODE_TO_PACK_FN[platform];
    promises.push(packFn(platformsInfo));
  }
  const results = await Promise.all(promises);

  if (results.some(performedPacking => performedPacking)) {
    console.log('All artifacts packed.');
  }
  else {
    // warn the user if we didn't pack anything
    console.warn('Nothing to do.');
  }
}

main(process.argv);
