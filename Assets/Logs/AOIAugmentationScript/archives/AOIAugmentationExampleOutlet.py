"""Example program to demonstrate how to send a multi-channel time series to
LSL."""
import random
import sys
import getopt
import string
import numpy as np
import time
from random import random as rand

from pylsl import StreamInfo, StreamOutlet, local_clock

def generate_zero_array(length, num_elements_to_set):
    array = np.zeros(length, dtype=np.float32)
    # select random 10 of them and set to 1
    indexes_to_set = random.sample(range(length), num_elements_to_set)
    array[indexes_to_set] = 1
    return array



def main(argv):
    letters = string.digits

    srate = 250
    name = 'StaticAOIAugmentationStateLSLInlet'
    print('Stream name is ' + name)
    type = 'EEG'
    n_channels = 50*25
    mysample = generate_zero_array(n_channels, 10)

    help_string = 'SendData.py -s <sampling_rate> -n <stream_name> -t <stream_type>'
    try:
        opts, args = getopt.getopt(argv, "hs:c:n:t:", longopts=["srate=", "channels=", "name=", "type"])
    except getopt.GetoptError:
        print(help_string)
        sys.exit(2)
    for opt, arg in opts:
        if opt == '-h':
            print(help_string)
            sys.exit()
        elif opt in ("-s", "--srate"):
            srate = float(arg)
        elif opt in ("-c", "--channels"):
            n_channels = int(arg)
        elif opt in ("-n", "--name"):
            name = arg
        elif opt in ("-t", "--type"):
            type = arg

    # first create a new stream info (here we set the name to BioSemi,
    # the content-type to EEG, 8 channels, 100 Hz, and float-valued data) The
    # last value would be the serial number of the device or some other more or
    # less locally unique identifier for the stream as far as available (you
    # could also omit it but interrupted connections wouldn't auto-recover)
    info = StreamInfo(name, type, n_channels, srate, 'float32', 'someuuid1234')

    # next make an outlet
    outlet = StreamOutlet(info)

    print("now sending data...")
    start_time = local_clock()
    sent_samples = 0
    while True:
        elapsed_time = local_clock() - start_time
        required_samples = int(srate * elapsed_time) - sent_samples
        for sample_ix in range(required_samples):
            # make a new random n_channels sample; this is converted into a
            # pylsl.vectorf (the data type that is expected by push_sample)
            # mysample = [rand()*100 for _ in range(n_channels)]
            # mysample = generate_zero_array(n_channels, 10)
            # now send it
            outlet.push_sample(mysample)
        sent_samples += required_samples
        # now send it and wait for a bit before trying again.
        time.sleep(0.01)


if __name__ == '__main__':
    main(sys.argv[1:])
