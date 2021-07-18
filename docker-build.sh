Version=$1
Tag=tonkica:$Version
Dockerfile=./dockerfile
Context=.

if [ -z "$1" ]
  then
    read -p "Must specify version to build: " Version
fi

docker build --build-arg Version=$Version --pull --rm -f $Dockerfile -t $Tag $Context