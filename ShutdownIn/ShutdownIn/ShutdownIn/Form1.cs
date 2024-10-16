using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ShutdownIn
{
    public partial class Form1 : Form
    {
        // PrivateFontCollection für benutzerdefinierte Schriftarten
        private PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        // NumericUpDown-Steuerelemente für Stunden und Minuten
        NumericUpDown hoursNumericUpDown;
        NumericUpDown minutesNumericUpDown;
        Button confirmButton;
        Button cancelButton; // Neuer Button zum Abbrechen des Shutdowns
        GroupBox groupBox;
        Label minutesLabel;
        Label hoursLabel;
        Label madeByLabel; // Label für "Made by Domi"
        Timer colorChangeTimer; // Timer für Farbänderung

        public Form1()
        {
            InitializeComponent();
            LoadCustomFonts();  // Benutzerdefinierte Schriftart laden
            InitializeFormControls();
        }

        private void LoadCustomFonts()
        {
            // Pfad zum Fonts-Ordner
            string fontsFolder = @"C:\Users\d.lauterbacher\Desktop\test\ShutdownIn\ShutdownIn\ShutdownIn\Fonts";

            // Alle TTF- und OTF-Dateien im Fonts-Ordner laden
            foreach (string fontFile in System.IO.Directory.GetFiles(fontsFolder, "*.ttf"))
            {
                privateFontCollection.AddFontFile(fontFile);
            }

            foreach (string fontFile in System.IO.Directory.GetFiles(fontsFolder, "*.otf"))
            {
                privateFontCollection.AddFontFile(fontFile);
            }
        }

        private void InitializeFormControls()
        {
            this.Size = new Size(500, 350); // Fenstergröße erhöhen
            this.Text = "PC Shutdown Timer - Dark Mode";
            this.BackColor = Color.Black;
            var neongreen = ColorTranslator.FromHtml("#00ff00");

            groupBox = new GroupBox();
            groupBox.Text = "Shutdown Timer";
            groupBox.Font = new Font(privateFontCollection.Families[0], 12, FontStyle.Bold); // Größere Schriftart
            groupBox.ForeColor = neongreen;
            groupBox.Location = new Point(30, 30);
            groupBox.Size = new Size(420, 250); // Größe der Gruppe erhöhen
            groupBox.BackColor = Color.Black;

            hoursLabel = new Label();
            hoursLabel.Text = "Hours:";
            hoursLabel.Font = new Font(privateFontCollection.Families[0], 10); // Schriftgröße erhöhen
            hoursLabel.ForeColor = neongreen;
            hoursLabel.Location = new System.Drawing.Point(20, 60);
            hoursLabel.Size = new System.Drawing.Size(100, 30);

            // NumericUpDown für Stunden
            hoursNumericUpDown = new NumericUpDown();
            hoursNumericUpDown.Location = new System.Drawing.Point(130, 60);
            hoursNumericUpDown.Size = new System.Drawing.Size(60, 30);
            hoursNumericUpDown.Minimum = 0; // Mindestwert 0 Stunden
            hoursNumericUpDown.Maximum = 23; // Maximalwert 23 Stunden
            hoursNumericUpDown.BackColor = Color.Black;
            hoursNumericUpDown.ForeColor = neongreen;

            minutesLabel = new Label();
            minutesLabel.Text = "Minutes:";
            minutesLabel.Font = new Font(privateFontCollection.Families[0], 10); // Schriftgröße erhöhen
            minutesLabel.ForeColor = neongreen;
            minutesLabel.Location = new System.Drawing.Point(20, 100);
            minutesLabel.Size = new System.Drawing.Size(100, 30);

            // NumericUpDown für Minuten
            minutesNumericUpDown = new NumericUpDown();
            minutesNumericUpDown.Location = new System.Drawing.Point(130, 100);
            minutesNumericUpDown.Size = new System.Drawing.Size(60, 30);
            minutesNumericUpDown.Minimum = 0; // Mindestwert 0 Minuten
            minutesNumericUpDown.Maximum = 59; // Maximalwert 59 Minuten
            minutesNumericUpDown.BackColor = Color.Black;
            minutesNumericUpDown.ForeColor = neongreen;

            confirmButton = new Button();
            confirmButton.Text = "Confirm";
            confirmButton.Font = new Font(privateFontCollection.Families[0], 12, FontStyle.Bold); // Größere Schriftart
            confirmButton.ForeColor = neongreen;
            confirmButton.BackColor = Color.Black;
            confirmButton.Location = new System.Drawing.Point(90, 150);
            confirmButton.Size = new System.Drawing.Size(120, 40); // Größe erhöhen
            confirmButton.FlatStyle = FlatStyle.Flat;

            // Eventhandler für den Bestätigen-Button hinzufügen
            confirmButton.Click += new EventHandler(this.ConfirmButton_Click);

            // Neuer Button zum Abbrechen des Shutdowns
            cancelButton = new Button();
            cancelButton.Text = "Cancel Shutdown";
            cancelButton.Font = new Font(privateFontCollection.Families[0], 12, FontStyle.Bold); // Größere Schriftart
            cancelButton.ForeColor = neongreen;
            cancelButton.BackColor = Color.Black;
            cancelButton.Location = new System.Drawing.Point(220, 150); // Rechts vom Bestätigen-Button
            cancelButton.Size = new System.Drawing.Size(160, 40); // Größe erhöhen
            cancelButton.FlatStyle = FlatStyle.Flat;

            // Eventhandler für den Abbrechen-Button hinzufügen
            cancelButton.Click += new EventHandler(this.CancelButton_Click);

            // Label für "Made by Domi" hinzufügen
            madeByLabel = new Label();
            madeByLabel.Text = "Made by Domi";
            madeByLabel.Font = new Font(privateFontCollection.Families[0], 8); // Schriftgröße anpassen
            madeByLabel.ForeColor = neongreen;
            madeByLabel.Location = new System.Drawing.Point(350, 300); // Position in der unteren rechten Ecke
            madeByLabel.Size = new System.Drawing.Size(120, 30);

            groupBox.Controls.Add(hoursLabel);
            groupBox.Controls.Add(hoursNumericUpDown); // Fügen Sie das NumericUpDown für Stunden zur Gruppe hinzu
            groupBox.Controls.Add(minutesLabel);
            groupBox.Controls.Add(minutesNumericUpDown); // Fügen Sie das NumericUpDown für Minuten zur Gruppe hinzu
            groupBox.Controls.Add(confirmButton);
            groupBox.Controls.Add(cancelButton); // Fügen Sie den Abbrechen-Button zur Gruppe hinzu

            this.Controls.Add(groupBox);
            this.Controls.Add(madeByLabel); // Label zum Formular hinzufügen

            // Timer für die Farbänderung initialisieren
            colorChangeTimer = new Timer();
            colorChangeTimer.Interval = 500; // 500 Millisekunden
            colorChangeTimer.Tick += new EventHandler(ColorChangeTimer_Tick);
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            // Variablen zum Speichern der eingegebenen Werte
            int inputHours = (int)hoursNumericUpDown.Value; // Stunden
            int inputMinutes = (int)minutesNumericUpDown.Value; // Minuten

            // Überprüfen, ob sowohl Stunden als auch Minuten 0 sind
            if (inputHours == 0 && inputMinutes == 0)
            {
                // Fehlermeldung anzeigen
                MessageBox.Show("Bitte geben Sie entweder Stunden oder Minuten ein.", "Eingabefehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Beenden Sie die Methode, ohne den Shutdown-Befehl auszuführen
            }

            // Berechnung der Zeit in Sekunden
            int totalSeconds = (inputHours * 3600) + (inputMinutes * 60);
            Shutdown(totalSeconds); // Funktion zum Herunterfahren aufrufen

            // Schriftfarbe kurzzeitig auf Rot ändern
            ChangeTextColor(Color.Red);

#if DEBUG
            AbortShutdown(); // Abbrechen nur im Debug-Modus
#endif
        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            // Funktion zum Abbrechen des Shutdowns aufrufen
            AbortShutdown();
            // Schriftfarbe auf grün ändern
            ChangeTextColor(Color.Green);
        }

        private void ChangeTextColor(Color color)
        {
            // Ändere die Schriftfarbe
            groupBox.ForeColor = color; 
            hoursNumericUpDown.ForeColor = color; 
            minutesNumericUpDown.ForeColor = color; 
            confirmButton.ForeColor = color;
            cancelButton.ForeColor = color; 
            minutesNumericUpDown.ForeColor = color;
            minutesLabel.ForeColor = color;
            hoursLabel.ForeColor = color;


            // Starte den Timer für die Rückänderung
            colorChangeTimer.Start();
        }

        private void ColorChangeTimer_Tick(object sender, EventArgs e)
        {
            // Timer stoppen
            colorChangeTimer.Stop();

            // Schriftfarbe zurücksetzen
            groupBox.ForeColor = ColorTranslator.FromHtml("#00ff00");
            hoursNumericUpDown.ForeColor = ColorTranslator.FromHtml("#00ff00"); 
            minutesNumericUpDown.ForeColor = ColorTranslator.FromHtml("#00ff00");
            confirmButton.ForeColor = ColorTranslator.FromHtml("#00ff00");
            cancelButton.ForeColor = ColorTranslator.FromHtml("#00ff00");
            minutesLabel.ForeColor = ColorTranslator.FromHtml("#00ff00");
            hoursLabel.ForeColor = ColorTranslator.FromHtml("#00ff00");

        }

        private void Shutdown(int seconds)
        {
            // Befehl zum Herunterfahren nach einer bestimmten Anzahl von Sekunden
            Process.Start("shutdown", $"/s /t {seconds}");
        }

        private void AbortShutdown()
        {
            // Befehl zum Abbrechen des Shutdowns
            Process.Start("shutdown", "/a");
        }
    }
}
