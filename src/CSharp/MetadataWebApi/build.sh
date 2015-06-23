SOLUTIONDIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
xbuild /verbosity:detailed $SOLUTIONDIR/MetadataWebApi.msbuild "$@"
