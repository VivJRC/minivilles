
namespace MinivilleBuildFinal
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_son = new System.Windows.Forms.Button();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btn_sfxVolume = new System.Windows.Forms.Button();
            this.btn_musicVolume = new System.Windows.Forms.Button();
            this.btn_masterVolume = new System.Windows.Forms.Button();
            this.mainVolume = new System.Windows.Forms.Label();
            this.btn_upperGlobal = new System.Windows.Forms.Button();
            this.btn_lowerGlobal = new System.Windows.Forms.Button();
            this.percentageSound = new System.Windows.Forms.Label();
            this.btn_upperSound = new System.Windows.Forms.Button();
            this.btn_lowerSound = new System.Windows.Forms.Button();
            this.soundVolume = new System.Windows.Forms.Label();
            this.globalVolume = new System.Windows.Forms.Label();
            this.percentageMusic = new System.Windows.Forms.Label();
            this.btn_upperMusic = new System.Windows.Forms.Button();
            this.btn_lowerMusic = new System.Windows.Forms.Button();
            this.MXP_SOUND = new AxWMPLib.AxWindowsMediaPlayer();
            this.MXP = new AxWMPLib.AxWindowsMediaPlayer();
            this.label1 = new System.Windows.Forms.Label();
            this.panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MXP_SOUND)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MXP)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_son
            // 
            this.btn_son.Location = new System.Drawing.Point(927, 7);
            this.btn_son.Margin = new System.Windows.Forms.Padding(2);
            this.btn_son.Name = "btn_son";
            this.btn_son.Size = new System.Drawing.Size(26, 28);
            this.btn_son.TabIndex = 31;
            this.btn_son.UseVisualStyleBackColor = true;
            this.btn_son.Click += new System.EventHandler(this.DisplayMenu);
            this.btn_son.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // panelMenu
            // 
            this.panelMenu.Controls.Add(this.label1);
            this.panelMenu.Controls.Add(this.btn_sfxVolume);
            this.panelMenu.Controls.Add(this.btn_musicVolume);
            this.panelMenu.Controls.Add(this.btn_masterVolume);
            this.panelMenu.Controls.Add(this.mainVolume);
            this.panelMenu.Controls.Add(this.btn_upperGlobal);
            this.panelMenu.Controls.Add(this.btn_lowerGlobal);
            this.panelMenu.Controls.Add(this.percentageSound);
            this.panelMenu.Controls.Add(this.btn_upperSound);
            this.panelMenu.Controls.Add(this.btn_lowerSound);
            this.panelMenu.Controls.Add(this.soundVolume);
            this.panelMenu.Controls.Add(this.globalVolume);
            this.panelMenu.Controls.Add(this.percentageMusic);
            this.panelMenu.Controls.Add(this.btn_upperMusic);
            this.panelMenu.Controls.Add(this.btn_lowerMusic);
            this.panelMenu.Location = new System.Drawing.Point(602, 7);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(2);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(320, 103);
            this.panelMenu.TabIndex = 31;
            // 
            // btn_sfxVolume
            // 
            this.btn_sfxVolume.Location = new System.Drawing.Point(158, 67);
            this.btn_sfxVolume.Margin = new System.Windows.Forms.Padding(2);
            this.btn_sfxVolume.Name = "btn_sfxVolume";
            this.btn_sfxVolume.Size = new System.Drawing.Size(26, 28);
            this.btn_sfxVolume.TabIndex = 30;
            this.btn_sfxVolume.UseVisualStyleBackColor = true;
            this.btn_sfxVolume.Click += new System.EventHandler(this.SfxVolume);
            this.btn_sfxVolume.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // btn_musicVolume
            // 
            this.btn_musicVolume.Location = new System.Drawing.Point(158, 34);
            this.btn_musicVolume.Margin = new System.Windows.Forms.Padding(2);
            this.btn_musicVolume.Name = "btn_musicVolume";
            this.btn_musicVolume.Size = new System.Drawing.Size(26, 28);
            this.btn_musicVolume.TabIndex = 29;
            this.btn_musicVolume.UseVisualStyleBackColor = true;
            this.btn_musicVolume.Click += new System.EventHandler(this.MusicVolume);
            this.btn_musicVolume.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // btn_masterVolume
            // 
            this.btn_masterVolume.Location = new System.Drawing.Point(158, 1);
            this.btn_masterVolume.Margin = new System.Windows.Forms.Padding(2);
            this.btn_masterVolume.Name = "btn_masterVolume";
            this.btn_masterVolume.Size = new System.Drawing.Size(26, 28);
            this.btn_masterVolume.TabIndex = 28;
            this.btn_masterVolume.UseVisualStyleBackColor = true;
            this.btn_masterVolume.Click += new System.EventHandler(this.MasterVolume);
            this.btn_masterVolume.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // mainVolume
            // 
            this.mainVolume.AutoSize = true;
            this.mainVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainVolume.Location = new System.Drawing.Point(234, 3);
            this.mainVolume.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.mainVolume.Name = "mainVolume";
            this.mainVolume.Size = new System.Drawing.Size(33, 31);
            this.mainVolume.TabIndex = 26;
            this.mainVolume.Text = "10";
            this.mainVolume.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.mainVolume.UseCompatibleTextRendering = true;
            // 
            // btn_upperGlobal
            // 
            this.btn_upperGlobal.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_upperGlobal.Location = new System.Drawing.Point(286, 3);
            this.btn_upperGlobal.Margin = new System.Windows.Forms.Padding(2);
            this.btn_upperGlobal.Name = "btn_upperGlobal";
            this.btn_upperGlobal.Size = new System.Drawing.Size(25, 28);
            this.btn_upperGlobal.TabIndex = 25;
            this.btn_upperGlobal.Text = "+";
            this.btn_upperGlobal.UseVisualStyleBackColor = true;
            this.btn_upperGlobal.Click += new System.EventHandler(this.UpperMainVolume);
            this.btn_upperGlobal.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // btn_lowerGlobal
            // 
            this.btn_lowerGlobal.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_lowerGlobal.Location = new System.Drawing.Point(189, 2);
            this.btn_lowerGlobal.Margin = new System.Windows.Forms.Padding(2);
            this.btn_lowerGlobal.Name = "btn_lowerGlobal";
            this.btn_lowerGlobal.Size = new System.Drawing.Size(26, 28);
            this.btn_lowerGlobal.TabIndex = 24;
            this.btn_lowerGlobal.Text = "-";
            this.btn_lowerGlobal.UseVisualStyleBackColor = true;
            this.btn_lowerGlobal.Click += new System.EventHandler(this.LowerMainVolume);
            this.btn_lowerGlobal.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // percentageSound
            // 
            this.percentageSound.AutoSize = true;
            this.percentageSound.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.percentageSound.Location = new System.Drawing.Point(232, 69);
            this.percentageSound.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.percentageSound.Name = "percentageSound";
            this.percentageSound.Size = new System.Drawing.Size(38, 26);
            this.percentageSound.TabIndex = 23;
            this.percentageSound.Text = "10";
            this.percentageSound.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_upperSound
            // 
            this.btn_upperSound.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_upperSound.Location = new System.Drawing.Point(286, 69);
            this.btn_upperSound.Margin = new System.Windows.Forms.Padding(2);
            this.btn_upperSound.Name = "btn_upperSound";
            this.btn_upperSound.Size = new System.Drawing.Size(25, 28);
            this.btn_upperSound.TabIndex = 22;
            this.btn_upperSound.Text = "+";
            this.btn_upperSound.UseVisualStyleBackColor = true;
            this.btn_upperSound.Click += new System.EventHandler(this.UpperSoundVolume);
            this.btn_upperSound.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // btn_lowerSound
            // 
            this.btn_lowerSound.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_lowerSound.Location = new System.Drawing.Point(189, 67);
            this.btn_lowerSound.Margin = new System.Windows.Forms.Padding(2);
            this.btn_lowerSound.Name = "btn_lowerSound";
            this.btn_lowerSound.Size = new System.Drawing.Size(26, 28);
            this.btn_lowerSound.TabIndex = 21;
            this.btn_lowerSound.Text = "-";
            this.btn_lowerSound.UseVisualStyleBackColor = true;
            this.btn_lowerSound.Click += new System.EventHandler(this.LowerSoundVolume);
            this.btn_lowerSound.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // soundVolume
            // 
            this.soundVolume.AutoSize = true;
            this.soundVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soundVolume.Location = new System.Drawing.Point(45, 68);
            this.soundVolume.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.soundVolume.Name = "soundVolume";
            this.soundVolume.Size = new System.Drawing.Size(110, 24);
            this.soundVolume.TabIndex = 20;
            this.soundVolume.Text = "sfx volume";
            // 
            // globalVolume
            // 
            this.globalVolume.AutoSize = true;
            this.globalVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.globalVolume.Location = new System.Drawing.Point(10, 2);
            this.globalVolume.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.globalVolume.Name = "globalVolume";
            this.globalVolume.Size = new System.Drawing.Size(146, 24);
            this.globalVolume.TabIndex = 18;
            this.globalVolume.Text = "master volume";
            // 
            // percentageMusic
            // 
            this.percentageMusic.AutoSize = true;
            this.percentageMusic.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.percentageMusic.Location = new System.Drawing.Point(232, 37);
            this.percentageMusic.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.percentageMusic.Name = "percentageMusic";
            this.percentageMusic.Size = new System.Drawing.Size(38, 26);
            this.percentageMusic.TabIndex = 17;
            this.percentageMusic.Text = "10";
            this.percentageMusic.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_upperMusic
            // 
            this.btn_upperMusic.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_upperMusic.Location = new System.Drawing.Point(286, 36);
            this.btn_upperMusic.Margin = new System.Windows.Forms.Padding(2);
            this.btn_upperMusic.Name = "btn_upperMusic";
            this.btn_upperMusic.Size = new System.Drawing.Size(25, 28);
            this.btn_upperMusic.TabIndex = 16;
            this.btn_upperMusic.Text = "+";
            this.btn_upperMusic.UseVisualStyleBackColor = true;
            this.btn_upperMusic.Click += new System.EventHandler(this.UpperMusicVolume);
            this.btn_upperMusic.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // btn_lowerMusic
            // 
            this.btn_lowerMusic.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_lowerMusic.Location = new System.Drawing.Point(189, 34);
            this.btn_lowerMusic.Margin = new System.Windows.Forms.Padding(2);
            this.btn_lowerMusic.Name = "btn_lowerMusic";
            this.btn_lowerMusic.Size = new System.Drawing.Size(26, 28);
            this.btn_lowerMusic.TabIndex = 15;
            this.btn_lowerMusic.Text = "-";
            this.btn_lowerMusic.UseVisualStyleBackColor = true;
            this.btn_lowerMusic.Click += new System.EventHandler(this.LowerMusicVolume);
            this.btn_lowerMusic.MouseEnter += new System.EventHandler(this.Hover);
            // 
            // MXP_SOUND
            // 
            this.MXP_SOUND.Enabled = true;
            this.MXP_SOUND.Location = new System.Drawing.Point(34, 104);
            this.MXP_SOUND.Margin = new System.Windows.Forms.Padding(2);
            this.MXP_SOUND.Name = "MXP_SOUND";
            this.MXP_SOUND.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MXP_SOUND.OcxState")));
            this.MXP_SOUND.Size = new System.Drawing.Size(218, 46);
            this.MXP_SOUND.TabIndex = 27;
            // 
            // MXP
            // 
            this.MXP.Enabled = true;
            this.MXP.Location = new System.Drawing.Point(34, 174);
            this.MXP.Margin = new System.Windows.Forms.Padding(2);
            this.MXP.Name = "MXP";
            this.MXP.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MXP.OcxState")));
            this.MXP.Size = new System.Drawing.Size(218, 46);
            this.MXP.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 24);
            this.label1.TabIndex = 31;
            this.label1.Text = "music volume";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 864);
            this.Controls.Add(this.btn_son);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.MXP_SOUND);
            this.Controls.Add(this.MXP);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MXP_SOUND)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MXP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_son;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btn_sfxVolume;
        private System.Windows.Forms.Button btn_musicVolume;
        private System.Windows.Forms.Button btn_masterVolume;
        private System.Windows.Forms.Label mainVolume;
        private System.Windows.Forms.Button btn_upperGlobal;
        private System.Windows.Forms.Button btn_lowerGlobal;
        private System.Windows.Forms.Label percentageSound;
        private System.Windows.Forms.Button btn_upperSound;
        private System.Windows.Forms.Button btn_lowerSound;
        private System.Windows.Forms.Label soundVolume;
        private System.Windows.Forms.Label globalVolume;
        private System.Windows.Forms.Label percentageMusic;
        private System.Windows.Forms.Button btn_upperMusic;
        private System.Windows.Forms.Button btn_lowerMusic;
        private AxWMPLib.AxWindowsMediaPlayer MXP_SOUND;
        private AxWMPLib.AxWindowsMediaPlayer MXP;
        private System.Windows.Forms.Label label1;
    }
}

