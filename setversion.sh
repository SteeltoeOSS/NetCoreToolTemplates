#!/usr/bin/env bash

set -e

prog=$(basename $0)
base_dir=$(cd $(dirname $0) && pwd)

if command -v gsed >/dev/null; then
  sed=gsed
else
  sed=sed
fi

usage() {
  cat <<EOF
USAGE
      $prog [-h] version
WHERE
      version
              Template version.
              If version includes ':' character, version string trimmed from
              that point.  E.g., "1.2.3-rc4:9876" becomes "1.2.3-rc4".
OPTIONS
      -h      Print this message.
DESCRIPTION
      Sets package version.
EOF
}

die() {
  echo $* 2>&1
  exit 1
}

while getopts ":h" opt ; do
  case $opt in
    h)
      usage
      exit
      ;;
    \?)
      die "invalid option -$OPTARG; run with -h for help"
      ;;
    :)
      die "option -$OPTARG requires an argument; run with -h for help"
      ;;
  esac
done
shift $(($OPTIND - 1))

if [ $# -eq 0 ]; then
  die "version not specified; run with -h for help"
fi

version=${1%#*}
shift

if [ $# -gt 0 ]; then
  die "too many args; run with -h for help"
fi

if ! $sed --version 2>/dev/null | grep 'GNU sed' >/dev/null; then
  die "This script requires GNU sed"
fi

echo "Setting version to $version"

$sed -i 's:<PackageVersion>.*</PackageVersion>:<PackageVersion>'$version'</PackageVersion>:' $base_dir/src/Steeltoe.NetCoreTool.Templates.csproj
