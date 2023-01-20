import sys
import impl

# Run execution
def run():

    print('Running build.py')

    assert len(sys.argv) == 2+1, 'Invalid argument count'

    _, source, devel_s = sys.argv
    devel = devel_s == 'true'

    impl.build(source, devel)

# Main execution
if __name__ == '__main__':
    run()