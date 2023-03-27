const path = require('path');

const GAME_NAME = 'MobileTowerDefense';
exports.GAME_NAME = GAME_NAME;

const PLATFORM_CODES = ['win', 'macos', 'linux', 'ios', 'android'];
exports.PLATFORM_CODES = PLATFORM_CODES;

function getPlatformSubdirectoryName(platform) {
  switch (platform) {
    case 'win':
      return 'Windows';
    case 'macos':
      return 'MacOS';
    case 'linux':
      return 'Linux';
    case 'ios':
      return 'Xcode';
    case 'android':
      return 'Android';
  }
}

function getPlatformDirectory(buildDir, platform) {
  const subdir = getPlatformSubdirectoryName(platform);
  return path.join(buildDir, subdir);
}

function getArtifactSubpath(buildDirBaseName, platform) {
  switch (platform) {
    case 'win':
      return '.';
    case 'macos':
      return `${GAME_NAME}.app`;
    case 'linux':
      return '.';
    case 'ios':
      return `${GAME_NAME}-Xcode-${buildDirBaseName}`;
    case 'android':
      return `${GAME_NAME}.apk`;
  }
}

function getArtifactPath(buildDirBaseName, buildDir, platform) {
  const platformDir = getPlatformDirectory(buildDir, platform);
  const artifactSubpath = getArtifactSubpath(buildDirBaseName, platform);
  return path.join(platformDir, artifactSubpath);
}



function getPackPathBaseName(buildDirBaseName, buildDir, platform) {
  switch (platform) {
    case 'win':
      return `${GAME_NAME}-Windows-${buildDirBaseName}`;
    case 'macos':
      return `${GAME_NAME}-MacOS-${buildDirBaseName}.app`;
    case 'linux':
      return `${GAME_NAME}-Linux-${buildDirBaseName}`;
    case 'ios':
      return `${GAME_NAME}-Xcode-${buildDirBaseName}`;
    case 'android':
      return `${GAME_NAME}-Android-${buildDirBaseName}.apk`;
  }
}

function getPackPath(buildDirBaseName, buildDir, platform) {
  const baseName = getPackPathBaseName(buildDirBaseName, buildDir, platform);
  return path.join(buildDir, baseName);
}

function getPlatformInfo(buildDirBaseName, buildDir, platform) {
  return {
    dir: getPlatformDirectory(buildDir, platform),
    artifactPath: getArtifactPath(buildDirBaseName, buildDir, platform),
    packPath: getPackPath(buildDirBaseName, buildDir, platform)
  };
}

function getInfoForPlatforms(buildDirBaseName, buildDir) {
  const ret = {};
  for (let i = 0; i < PLATFORM_CODES.length; ++i) {
    const platform = PLATFORM_CODES[i];
    ret[platform] = getPlatformInfo(buildDirBaseName, buildDir, platform);
  }
  return ret;
}
exports.getInfoForPlatforms = getInfoForPlatforms;
