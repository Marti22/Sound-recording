using System;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.FileFormats;

namespace Sound_recording
{
    public partial class Form1 : Form
    {
        // WaveIn - stream for writing
        private WaveIn _waveIn;
        // Class to write to the file
        private WaveFileWriter _writer;
        // Filename for the record
        private string outputFile = "file.wav";

        public Form1()
        {
            InitializeComponent();
        }
        // Get data from the input buffer
        void waveInDataAvaible(object sender, WaveInEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<WaveInEventArgs>(waveInDataAvaible), sender, e);
            }
            else
            {
                // Write the data from the buffer to the file
                _writer.WriteData(e.Buffer, 0, e.BytesRecorded);
            }
        }

        void StopRecording()
        {
            // End the record
            MessageBox.Show(@"Stop recording");
            _waveIn.StopRecording();
        }
        // End of record
        private void waveInRecordingStopped(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(waveInRecordingStopped), sender, e);
            }
            else
            {
                _waveIn.Dispose();
                _waveIn = null;
                _writer.Close();
                _writer = null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        // Start recording - the button click handler
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(@"Start recording");
                _waveIn = new WaveIn();
                // The default device for writing 
                _waveIn.DeviceNumber = 0;
                // Attach a handler to the DataAvailable event that occurs when there is data to write
                _waveIn.DataAvailable += waveInDataAvaible;
                // Attach the write termination handler
                _waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(waveInRecordingStopped);
                // Format of the wav file - takes parameters - the sampling frequency and the number of channels (here mono)
                _waveIn.WaveFormat = new WaveFormat(8000, 1);
                // Initialize the WaveFileWriter object
                _writer = new WaveFileWriter(outputFile, _waveIn.WaveFormat);
                // Start recording
                _waveIn.StartRecording();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        // Interrupt the record - the second button click handler
        private void button2_Click(object sender, EventArgs e)
        {
            if (_waveIn != null)
            {
                StopRecording();
            }
        }
    }

}

