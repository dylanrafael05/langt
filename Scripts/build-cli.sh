cd "../Core/langt-cli"

for NAME in "linux-x64" "win-x64"
do
    echo "Building for $NAME"

    FOLDER="../../Builds/$NAME/cli"

    rm $FOLDER -r 
    dotnet publish -o "$FOLDER" -r "$NAME" --self-contained true
done