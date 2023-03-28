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
    fs.rm(subdirA, { recursive: true, force: true }),
    fs.rm(subdirB, { recursive: true, force: true })
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

async function packWindows(platformsInfo) {
  if (!(await shouldPackDirectory(platformsInfo.win.dir))) {
    return false;
  }
  await fs.cp(platformsInfo.win.dir, platformsInfo.win.packPath, { recursive: true });
  await removeNoShipSubdirs(platformsInfo.win.packPath);
  await zip(platformsInfo.win.packPath, true);
  return true;
}

async function packMacOS(platformsInfo) {
  if (!(await shouldPackDirectory(platformsInfo.macos.dir))) {
    return false;
  }
  await fs.cp(platformsInfo.macos.artifactPath, platformsInfo.macos.packPath, { recursive: true });
  await zip(platformsInfo.macos.packPath, true);
  return true;
}

async function packLinux(platformsInfo) {
  if (!(await shouldPackDirectory(platformsInfo.linux.dir))) {
    return false;
  }
  await fs.cp(platformsInfo.linux.dir, platformsInfo.linux.packPath, { recursive: true });
  await removeNoShipSubdirs(platformsInfo.linux.packPath);
  await zip(platformsInfo.linux.packPath, true);
  return true;
}

async function packIOS(platformsInfo) {
  if (!(await dirExists(platformsInfo.ios.dir))) {
    return false;
  }
  if (!(await shouldPackDirectory(platformsInfo.ios.artifactPath))) {
    return false;
  }
  await fs.cp(platformsInfo.ios.artifactPath, platformsInfo.ios.packPath, { recursive: true });
  await zip(platformsInfo.ios.packPath, true);
  return true;
}

async function packAndroid(platformsInfo) {
  if (!(await shouldPackDirectory(platformsInfo.android.dir))) {
    return false;
  }
  await fs.copyFile(platformsInfo.android.artifactPath, platformsInfo.android.packPath);
  await zip(platformsInfo.android.packPath, false);
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
    console.log('Done.');
  }
  else {
    // warn the user if we didn't pack anything
    console.warn('Nothing to do.');
  }
}

main(process.argv);
