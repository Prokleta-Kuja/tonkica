Version=$1
if [ -z "$1" ]
  then
    read -p "Must specify version to build: " Version
fi

Tag=tonkica:$Version
Dockerfile=./dockerfile
Context=.

docker build --build-arg Version=$Version --pull --rm -f $Dockerfile -t $Tag $Context