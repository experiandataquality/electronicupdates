SOLUTIONDIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
xbuild /verbosity:minimal $SOLUTIONDIR/MetadataWebApi.msbuild "$@"
