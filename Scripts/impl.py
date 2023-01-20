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

sources : list[str] = [
    'langt-cli',
    'langt-core',
]

builds : str = r'Builds'

#################################
# FUNCTIONS
#################################
def clear(dir: str):
    if os.path.exists(dir):
        shutil.rmtree(dir)

#################################
# MAIN BUILD SCRIPT
#################################
def build(source: str, devel: bool):

    # Handle 'all'
    if source == 'all':

        for s in sources:
            build(s, devel)

        return

    # Ensure proper source
    assert source in sources

    # Begin actual build
    ccwd = os.getcwd()
    os.chdir(rf'Core\{source}')

    if not devel:

        # Full publish
        for target in targets:

            print(f'Building {source} for {target}\n')

            fol = os.path.join(ccwd, builds, source, target)

            clear(fol)
            os.system(rf'dotnet publish -o {fol} -r {target} --sc true')

            print('-' * 50)
            print(f'Zipping built files')

            with zipfile.ZipFile(os.path.join(ccwd, builds, f'{source}.{target}.zip'), 'w') as new_zip:
                for file in os.scandir(fol):
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

