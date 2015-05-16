SOLUTIONDIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
ant -buildfile $SOLUTIONDIR/build.xml "$@"

