import os
import shutil
import zipfile

#################################
# CONSTANTS
#################################
targets : list[str] = [
    'win-x64',
    'linux-x64',
]

sources : dict[str, str] = {
    'langt-cli': r'Core\langt-cli',
    'langt-core': r'Core\langt-core',
}

builds : str = r'Builds'

#################################
# FUNCTIONS
#################################
def clear(dir: str):
    if os.path.exists(dir):
        shutil.rmtree(dir)

#################################
# MAIN BUILD SCRIPTS
#################################
def build_source(source: str, devel: bool):

    # Handle 'all'
    if source == 'all':

        for s in sources:
            build_source(s, devel)

        return

    # Ensure proper source
    assert source in sources

    # Begin actual build
    ccwd = os.getcwd()
    os.chdir(sources[source])

    if not devel:

        # Full publish
        for target in targets:

            print(f'Building {source} for {target}\n')

            fol = os.path.join(ccwd, builds, source, target)

            clear(fol)
            os.system(rf'dotnet publish -o {fol} -r {target} --sc true')

            print('-' * 50)
            print(f'Zipping build')

            with zipfile.ZipFile(os.path.join(ccwd, builds, f'{source}.{target}.zip'), 'w') as new_zip:
                for file in os.scandir(fol):
                    print(f'Adding file {file.name}')
                    new_zip.write(file.path)

            print('-' * 50)

    else:
        
        # Simple, one step build
        print(f'Building develop version of {source}')

        fol = os.path.join(ccwd, builds, source, 'Debug')

        clear(fol)
        os.system(rf'dotnet build -o {fol} --sc false')

    # Return cwd
    os.chdir(ccwd)
