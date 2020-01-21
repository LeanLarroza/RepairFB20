using FirebirdSql.Data.FirebirdClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RepairFB20
{
    public partial class Form1 : Form
    {
        protected FbConnection DatabaseOriginale;
        protected string UserDbOriginale;
        protected string PasswordDbOriginale;
        protected string UserDbDestino;
        protected string PasswordDbDestino = "";
        protected string PercorsoDbOriginale = "";
        protected string PercorsoDbDestino;
        protected bool IsConnectionDbOriginale = false;
        protected bool IsConnectionDbDestino = false;
        public FbConnection ConnectionDbOriginale;
        public FbConnection ConnectionDbDestino;


        public Form1()
        {
            InitializeComponent();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            progresso.Value = 0;
            progresso.Maximum = 2;
            PercorsoDbOriginale = @"c:\DatabaseOriginale\trilogis.fb20";
            PercorsoDbDestino = @"c:\trilogis\trilogis.fb20";

            UserDbOriginale = Interaction.InputBox("Inserire USER database originale","Repair FB20","SYSDBA");
            PasswordDbOriginale = Interaction.InputBox("Inserire PASSWORD database originale", "Repair FB20", "masterkey");
            while (!IsConnectionDbOriginale)
            {
                try
                {
                    ConnectionDbOriginale = new FbConnection("User = " + UserDbOriginale + "; Password = " + PasswordDbOriginale + "; Database = " + PercorsoDbOriginale + "; DataSource = localhost;");
                    ConnectionDbOriginale.Open();
                    IsConnectionDbOriginale = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    UserDbOriginale = Interaction.InputBox("Inserire USER database originale", "Repair FB20", "SYSDBA");
                    PasswordDbOriginale = Interaction.InputBox("Inserire PASSWORD database originale", "Repair FB20", "masterkey");
                    IsConnectionDbOriginale = false;
                }
            }

            progresso.PerformStep();

            UserDbDestino = Interaction.InputBox("Inserire USER database destino", "Repair FB20", "SYSDBA");
            PasswordDbDestino = Interaction.InputBox("Inserire PASSWORD database destino", "Repair FB20", "masterkey");
            while (!IsConnectionDbDestino)
            {
                try
                {
                    ConnectionDbDestino = new FbConnection("User = " + UserDbDestino + "; Password = " + PasswordDbDestino + "; Database = " + PercorsoDbDestino + "; DataSource = localhost;");
                    ConnectionDbDestino.Open();
                    IsConnectionDbDestino = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    UserDbDestino = Interaction.InputBox("Inserire USER database destino", "Repair FB20", "SYSDBA");
                    PasswordDbDestino = Interaction.InputBox("Inserire PASSWORD database destino", "Repair FB20", "masterkey");
                    IsConnectionDbDestino = false;
                }
            }
            progresso.PerformStep();
            MessageBox.Show("Operazione completata.","Repair FB20");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            progresso.Value = 0;
            progresso.Maximum = 7;
            try
            {
                FbCommand CancPagRigheZero = new FbCommand("DELETE FROM PAGAMENTIRIGHE WHERE LOTTORIGAID = 0", ConnectionDbOriginale);
                CancPagRigheZero.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            progresso.PerformStep();
            try
            {
                FbCommand CancPagLottiZero = new FbCommand("DELETE FROM PAGAMENTILOTTI WHERE LOTTOID = 0", ConnectionDbOriginale);
                CancPagLottiZero.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            progresso.PerformStep();
            try
            {
                FbCommand CancDocRigheZero = new FbCommand("DELETE FROM DOCUMENTIRIGHE WHERE LOTTORIGAID = 0", ConnectionDbOriginale);
                CancDocRigheZero.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            progresso.PerformStep();
            try
            {
                FbCommand CancDocLottiZero = new FbCommand("DELETE FROM DOCUMENTILOTTI WHERE LOTTOID = 0", ConnectionDbOriginale);
                CancDocLottiZero.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            progresso.PerformStep();
            try
            {
                FbCommand CancLottirigheLavZero = new FbCommand("DELETE FROM LOTTIRIGHELAVORAZIONI WHERE LOTTORIGAID = 0", ConnectionDbOriginale);
                CancLottirigheLavZero.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            progresso.PerformStep();
            try
            {
                FbCommand CancLottirigheZero = new FbCommand("DELETE FROM LOTTIRIGHE WHERE LOTTORIGAID = 0", ConnectionDbOriginale);
                CancLottirigheZero.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            progresso.PerformStep();
            try
            {
                FbCommand CancLottiZero = new FbCommand("DELETE FROM LOTTI WHERE LOTTOID = 0", ConnectionDbOriginale);
                CancLottiZero.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            progresso.PerformStep();
            MessageBox.Show("Operazione completata.", "Repair FB20");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            progresso.Value = 0;
            progresso.Maximum = 1;
            FbCommand DisableTriggers = new FbCommand(" update RDB$TRIGGERS set RDB$TRIGGER_INACTIVE=1 where RDB$TRIGGER_NAME in (select A.RDB$TRIGGER_NAME from RDB$TRIGGERS A left join RDB$CHECK_CONSTRAINTS B ON B.RDB$TRIGGER_NAME = A.RDB$TRIGGER_NAME where((A.RDB$SYSTEM_FLAG = 0) or(A.RDB$SYSTEM_FLAG is null)) and(b.rdb$trigger_name is null) AND (NOT(A.RDB$TRIGGER_NAME LIKE 'RDB$%'))); ", ConnectionDbDestino);
            DisableTriggers.ExecuteNonQuery();
            progresso.PerformStep();
            MessageBox.Show("Operazione completata.", "Repair FB20");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            progresso.Value = 0;
            progresso.Maximum = 1;
            FbCommand AggTrNumeroSco = new FbCommand("ALTER TRIGGER TRG_GEN_LOTTOID ACTIVE BEFORE INSERT POSITION 0 AS DECLARE VARIABLE LASTID INTEGER; DECLARE VARIABLE LASTNUMERO INTEGER; DECLARE VARIABLE LASTNUMEROCOUNT INTEGER; DECLARE VARIABLE NEWNUM INTEGER; DECLARE VARIABLE OLDNUM INTEGER; begin NEW.LOTTOID = GEN_ID(GEN_LOTTOID, 1); NEW.LOTTONUMERO = GEN_ID(GEN_LOTTONUM, 1); SELECT COUNT(LOTTONUMERO) FROM LOTTIATTIVI  WHERE(LOTTONUMERO = NEW.LOTTONUMERO AND ((LOTTORECAPITOID = 0) or(LOTTORECAPITOID is null) or((: OLDNUM = -999) AND(LOTTORECAPITOID = NEW.LOTTORECAPITOID)))) INTO LASTNUMEROCOUNT; WHILE(:LASTNUMEROCOUNT > 0) DO BEGIN NEW.LOTTONUMERO = GEN_ID(GEN_LOTTONUM, 1); SELECT COUNT(LOTTONUMERO) FROM LOTTIATTIVI  WHERE(LOTTONUMERO = NEW.LOTTONUMERO AND((LOTTORECAPITOID = 0) or(LOTTORECAPITOID is null) or ((: OLDNUM = -999) AND(LOTTORECAPITOID = NEW.LOTTORECAPITOID)))) INTO LASTNUMEROCOUNT; END END;", ConnectionDbDestino);
            AggTrNumeroSco.ExecuteNonQuery();
            progresso.PerformStep();
            MessageBox.Show("Operazione completata.", "Repair FB20");
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            progresso.Value = 0;
            progresso.Maximum = 23;
            FbCommand CancPagRighe = new FbCommand("DELETE FROM PAGAMENTIRIGHE", ConnectionDbDestino);
            CancPagRighe.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancPagLotti = new FbCommand("DELETE FROM PAGAMENTILOTTI", ConnectionDbDestino);
            CancPagLotti.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancDocRighe = new FbCommand("DELETE FROM DOCUMENTIRIGHE", ConnectionDbDestino);
            CancDocRighe.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancDocLotti = new FbCommand("DELETE FROM DOCUMENTILOTTI", ConnectionDbDestino);
            CancDocLotti.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancMovimBox = new FbCommand("DELETE FROM MOVIMBOX", ConnectionDbDestino);
            CancMovimBox.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancLottirigheLav = new FbCommand("DELETE FROM LOTTIRIGHELAVORAZIONI", ConnectionDbDestino);
            CancLottirigheLav.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancLottirighe = new FbCommand("DELETE FROM LOTTIRIGHE", ConnectionDbDestino);
            CancLottirighe.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancLotti = new FbCommand("DELETE FROM LOTTI", ConnectionDbDestino);
            CancLotti.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancClienti = new FbCommand("DELETE FROM CLIENTI", ConnectionDbDestino);
            CancClienti.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancLavArt = new FbCommand("DELETE FROM LAVORAZIONIARTICOLI", ConnectionDbDestino);
            CancLavArt.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancLav = new FbCommand("DELETE FROM LAVORAZIONI", ConnectionDbDestino);
            CancLav.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancDimArt = new FbCommand("DELETE FROM DIMENSIONIARTICOLI", ConnectionDbDestino);
            CancDimArt.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancArt = new FbCommand("DELETE FROM ARTICOLI", ConnectionDbDestino);
            CancArt.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancVarianti = new FbCommand("DELETE FROM VARIANTI", ConnectionDbDestino);
            CancVarianti.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancPVVarianti = new FbCommand("DELETE FROM PAGINEVIDEOVARIANTI", ConnectionDbDestino);
            CancPVVarianti.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancPVArt = new FbCommand("DELETE FROM PAGINEVIDEOARTICOLI", ConnectionDbDestino);
            CancPVArt.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancPV = new FbCommand("DELETE FROM PAGINEVIDEO", ConnectionDbDestino);
            CancPV.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancConOp = new FbCommand("DELETE FROM CONNESSIONIOPERATORI", ConnectionDbDestino);
            CancConOp.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancOp = new FbCommand("DELETE FROM OPERATORI", ConnectionDbDestino);
            CancOp.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancMagazzini = new FbCommand("DELETE FROM MAGAZZINI", ConnectionDbDestino);
            CancMagazzini.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancMagBox = new FbCommand("DELETE FROM MAGAZZINIBOX", ConnectionDbDestino);
            CancMagBox.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancMovimLabLotti = new FbCommand("DELETE FROM MOVIMLABLOTTI", ConnectionDbDestino);
            CancMovimLabLotti.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancMagaMisure = new FbCommand("DELETE FROM MAGAZZINIMISUREMAX", ConnectionDbDestino);
            CancMagaMisure.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancModifLottirighe = new FbCommand("DELETE FROM MODIFICATORILOTTIRIGHE", ConnectionDbDestino);
            CancModifLottirighe.ExecuteNonQuery();
            progresso.PerformStep();
            FbCommand CancModifLotti = new FbCommand("DELETE FROM MODIFICATORILOTTI", ConnectionDbDestino);
            CancModifLotti.ExecuteNonQuery();
            progresso.PerformStep();
            MessageBox.Show("Operazione completata.", "Repair FB20");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"C:\Firebird"))
            {
                MessageBox.Show("Copiare la cartella \"" + Application.StartupPath + "\\Firebird\" su C:\\ prima di avviare il programma di conversione.", "Repair FB20");
                Environment.Exit(0);
            }
            progresso.Value = 0;
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            progresso.Value = 0;
            progresso.Maximum = 23;

            System.Diagnostics.Process CopiaPagineVideo = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiaPagineVideoINFO = new System.Diagnostics.ProcessStartInfo();
            CopiaPagineVideoINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiaPagineVideoINFO.FileName = "cmd.exe";
            CopiaPagineVideoINFO.WorkingDirectory = @"C:\Firebird";
            CopiaPagineVideoINFO.Arguments = @"/C fbexport -S -V paginevideo -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V paginevideo -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            CopiaPagineVideo.StartInfo = CopiaPagineVideoINFO;
            CopiaPagineVideo.Start();
            CopiaPagineVideo.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiapaginevideoarticoli = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiapaginevideoarticoliINFO = new System.Diagnostics.ProcessStartInfo();
            CopiapaginevideoarticoliINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiapaginevideoarticoliINFO.FileName = "cmd.exe";
            CopiapaginevideoarticoliINFO.WorkingDirectory = @"C:\Firebird";
            CopiapaginevideoarticoliINFO.Arguments = @"/C fbexport -S -V paginevideoarticoli -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V paginevideoarticoli -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiapaginevideoarticoli.StartInfo = CopiapaginevideoarticoliINFO;
            Copiapaginevideoarticoli.Start();
            Copiapaginevideoarticoli.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiapaginevideovarianti = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiapaginevideovariantiINFO = new System.Diagnostics.ProcessStartInfo();
            CopiapaginevideovariantiINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiapaginevideovariantiINFO.FileName = "cmd.exe";
            CopiapaginevideovariantiINFO.WorkingDirectory = @"C:\Firebird";
            CopiapaginevideovariantiINFO.Arguments = @"/C fbexport -S -V paginevideovarianti -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V paginevideovarianti -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiapaginevideovarianti.StartInfo = CopiapaginevideovariantiINFO;
            Copiapaginevideovarianti.Start();
            Copiapaginevideovarianti.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiavarianti = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiavariantiINFO = new System.Diagnostics.ProcessStartInfo();
            CopiavariantiINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiavariantiINFO.FileName = "cmd.exe";
            CopiavariantiINFO.WorkingDirectory = @"C:\Firebird";
            CopiavariantiINFO.Arguments = @"/C fbexport -S -V varianti -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V varianti -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiavarianti.StartInfo = CopiavariantiINFO;
            Copiavarianti.Start();
            Copiavarianti.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiaarticoli = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiaarticoliINFO = new System.Diagnostics.ProcessStartInfo();
            CopiaarticoliINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiaarticoliINFO.FileName = "cmd.exe";
            CopiaarticoliINFO.WorkingDirectory = @"C:\Firebird";
            CopiaarticoliINFO.Arguments = @"/C fbexport -S -V articoli -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V articoli -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiaarticoli.StartInfo = CopiaarticoliINFO;
            Copiaarticoli.Start();
            Copiaarticoli.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiadimensioniarticoli = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiadimensioniarticoliINFO = new System.Diagnostics.ProcessStartInfo();
            CopiadimensioniarticoliINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiadimensioniarticoliINFO.FileName = "cmd.exe";
            CopiadimensioniarticoliINFO.WorkingDirectory = @"C:\Firebird";
            CopiadimensioniarticoliINFO.Arguments = @"/C fbexport -S -V dimensioniarticoli -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V dimensioniarticoli -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiadimensioniarticoli.StartInfo = CopiadimensioniarticoliINFO;
            Copiadimensioniarticoli.Start();
            Copiadimensioniarticoli.WaitForExit();
            progresso.PerformStep();


            System.Diagnostics.Process Copialavorazioni = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopialavorazioniINFO = new System.Diagnostics.ProcessStartInfo();
            CopialavorazioniINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopialavorazioniINFO.FileName = "cmd.exe";
            CopialavorazioniINFO.WorkingDirectory = @"C:\Firebird";
            CopialavorazioniINFO.Arguments = @"/C fbexport -S -V lavorazioni -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V lavorazioni -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copialavorazioni.StartInfo = CopialavorazioniINFO;
            Copialavorazioni.Start();
            Copialavorazioni.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copialavorazioniarticoli = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopialavorazioniarticoliINFO = new System.Diagnostics.ProcessStartInfo();
            CopialavorazioniarticoliINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopialavorazioniarticoliINFO.FileName = "cmd.exe";
            CopialavorazioniarticoliINFO.WorkingDirectory = @"C:\Firebird";
            CopialavorazioniarticoliINFO.Arguments = @"/C fbexport -S -V lavorazioniarticoli -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V lavorazioniarticoli -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copialavorazioniarticoli.StartInfo = CopialavorazioniarticoliINFO;
            Copialavorazioniarticoli.Start();
            Copialavorazioniarticoli.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiaclienti = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiaclientiINFO = new System.Diagnostics.ProcessStartInfo();
            CopiaclientiINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiaclientiINFO.FileName = "cmd.exe";
            CopiaclientiINFO.WorkingDirectory = @"C:\Firebird";
            CopiaclientiINFO.Arguments = @"/C fbexport -S -V clienti -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V clienti -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiaclienti.StartInfo = CopiaclientiINFO;
            Copiaclienti.Start();
            Copiaclienti.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copialotti = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopialottiINFO = new System.Diagnostics.ProcessStartInfo();
            CopialottiINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopialottiINFO.FileName = "cmd.exe";
            CopialottiINFO.WorkingDirectory = @"C:\Firebird";
            CopialottiINFO.Arguments = @"/C fbexport -S -V lotti -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V lotti -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copialotti.StartInfo = CopialottiINFO;
            Copialotti.Start();
            Copialotti.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copialottirighe = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopialottirigheINFO = new System.Diagnostics.ProcessStartInfo();
            CopialottirigheINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopialottirigheINFO.FileName = "cmd.exe";
            CopialottirigheINFO.WorkingDirectory = @"C:\Firebird";
            CopialottirigheINFO.Arguments = @"/C fbexport -S -V lottirighe -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V lottirighe -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copialottirighe.StartInfo = CopialottirigheINFO;
            Copialottirighe.Start();
            Copialottirighe.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copialottirighelavorazioni = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopialottirighelavorazioniINFO = new System.Diagnostics.ProcessStartInfo();
            CopialottirighelavorazioniINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopialottirighelavorazioniINFO.FileName = "cmd.exe";
            CopialottirighelavorazioniINFO.WorkingDirectory = @"C:\Firebird";
            CopialottirighelavorazioniINFO.Arguments = @"/C fbexport -S -V lottirighelavorazioni -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V lottirighelavorazioni -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copialottirighelavorazioni.StartInfo = CopialottirighelavorazioniINFO;
            Copialottirighelavorazioni.Start();
            Copialottirighelavorazioni.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiamovimbox = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiamovimboxINFO = new System.Diagnostics.ProcessStartInfo();
            CopiamovimboxINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiamovimboxINFO.FileName = "cmd.exe";
            CopiamovimboxINFO.WorkingDirectory = @"C:\Firebird";
            CopiamovimboxINFO.Arguments = @"/C fbexport -S -V movimbox -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V movimbox -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiamovimbox.StartInfo = CopiamovimboxINFO;
            Copiamovimbox.Start();
            Copiamovimbox.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiadocumentilotti = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiadocumentilottiINFO = new System.Diagnostics.ProcessStartInfo();
            CopiadocumentilottiINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiadocumentilottiINFO.FileName = "cmd.exe";
            CopiadocumentilottiINFO.WorkingDirectory = @"C:\Firebird";
            CopiadocumentilottiINFO.Arguments = @"/C fbexport -S -V documentilotti -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V documentilotti -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiadocumentilotti.StartInfo = CopiadocumentilottiINFO;
            Copiadocumentilotti.Start();
            Copiadocumentilotti.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiadocumentirighe = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiadocumentirigheINFO = new System.Diagnostics.ProcessStartInfo();
            CopiadocumentirigheINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiadocumentirigheINFO.FileName = "cmd.exe";
            CopiadocumentirigheINFO.WorkingDirectory = @"C:\Firebird";
            CopiadocumentirigheINFO.Arguments = @"/C fbexport -S -V documentirighe -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V documentirighe -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiadocumentirighe.StartInfo = CopiadocumentirigheINFO;
            Copiadocumentirighe.Start();
            Copiadocumentirighe.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiapagamentilotti = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiapagamentilottiINFO = new System.Diagnostics.ProcessStartInfo();
            CopiapagamentilottiINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiapagamentilottiINFO.FileName = "cmd.exe";
            CopiapagamentilottiINFO.WorkingDirectory = @"C:\Firebird";
            CopiapagamentilottiINFO.Arguments = @"/C fbexport -S -V pagamentilotti -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V pagamentilotti -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiapagamentilotti.StartInfo = CopiapagamentilottiINFO;
            Copiapagamentilotti.Start();
            Copiapagamentilotti.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiapagamentirighe = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiapagamentirigheINFO = new System.Diagnostics.ProcessStartInfo();
            CopiapagamentirigheINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiapagamentirigheINFO.FileName = "cmd.exe";
            CopiapagamentirigheINFO.WorkingDirectory = @"C:\Firebird";
            CopiapagamentirigheINFO.Arguments = @"/C fbexport -S -V pagamentirighe -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V pagamentirighe -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiapagamentirighe.StartInfo = CopiapagamentirigheINFO;
            Copiapagamentirighe.Start();
            Copiapagamentirighe.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiaoperatori = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiaoperatoriINFO = new System.Diagnostics.ProcessStartInfo();
            CopiaoperatoriINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiaoperatoriINFO.FileName = "cmd.exe";
            CopiaoperatoriINFO.WorkingDirectory = @"C:\Firebird";
            CopiaoperatoriINFO.Arguments = @"/C fbexport -S -V operatori -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V operatori -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiaoperatori.StartInfo = CopiaoperatoriINFO;
            Copiaoperatori.Start();
            Copiaoperatori.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiamagazzini = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiamagazziniINFO = new System.Diagnostics.ProcessStartInfo();
            CopiamagazziniINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiamagazziniINFO.FileName = "cmd.exe";
            CopiamagazziniINFO.WorkingDirectory = @"C:\Firebird";
            CopiamagazziniINFO.Arguments = @"/C fbexport -S -V magazzini -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V magazzini -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiamagazzini.StartInfo = CopiamagazziniINFO;
            Copiamagazzini.Start();
            Copiamagazzini.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiamagazzinibox = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiamagazziniboxINFO = new System.Diagnostics.ProcessStartInfo();
            CopiamagazziniboxINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiamagazziniboxINFO.FileName = "cmd.exe";
            CopiamagazziniboxINFO.WorkingDirectory = @"C:\Firebird";
            CopiamagazziniboxINFO.Arguments = @"/C fbexport -S -V magazzinibox -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V magazzinibox -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiamagazzinibox.StartInfo = CopiamagazziniboxINFO;
            Copiamagazzinibox.Start();
            Copiamagazzinibox.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiamagmisure = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiamagmisureINFO = new System.Diagnostics.ProcessStartInfo();
            CopiamagmisureINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiamagmisureINFO.FileName = "cmd.exe";
            CopiamagmisureINFO.WorkingDirectory = @"C:\Firebird";
            CopiamagmisureINFO.Arguments = @"/C fbexport -S -V magazzinimisuremax -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V magazzinimisuremax -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiamagmisure.StartInfo = CopiamagmisureINFO;
            Copiamagmisure.Start();
            Copiamagmisure.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiamovimlablotti = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiamovimlablottiINFO = new System.Diagnostics.ProcessStartInfo();
            CopiamovimlablottiINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiamovimlablottiINFO.FileName = "cmd.exe";
            CopiamovimlablottiINFO.WorkingDirectory = @"C:\Firebird";
            CopiamovimlablottiINFO.Arguments = @"/C fbexport -S -V movimlablotti -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V movimlablotti -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiamovimlablotti.StartInfo = CopiamovimlablottiINFO;
            Copiamovimlablotti.Start();
            Copiamovimlablotti.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiamodiflottirighe = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiamodiflottirigheINFO = new System.Diagnostics.ProcessStartInfo();
            CopiamodiflottirigheINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiamodiflottirigheINFO.FileName = "cmd.exe";
            CopiamodiflottirigheINFO.WorkingDirectory = @"C:\Firebird";
            CopiamodiflottirigheINFO.Arguments = @"/C fbexport -S -V modificatorilottirighe -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V modificatorilottirighe -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiamodiflottirighe.StartInfo = CopiamodiflottirigheINFO;
            Copiamodiflottirighe.Start();
            Copiamodiflottirighe.WaitForExit();
            progresso.PerformStep();

            System.Diagnostics.Process Copiamodiflotti = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo CopiamodiflottiINFO = new System.Diagnostics.ProcessStartInfo();
            CopiamodiflottiINFO.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            CopiamodiflottiINFO.FileName = "cmd.exe";
            CopiamodiflottiINFO.WorkingDirectory = @"C:\Firebird";
            CopiamodiflottiINFO.Arguments = @"/C fbexport -S -V modificatorilotti -D " + PercorsoDbOriginale + " -H localhost -P " + PasswordDbOriginale + " -F - | fbexport -I -V modificatorilotti -D " + PercorsoDbDestino + " -H localhost -P " + PasswordDbDestino + " -F - -R";
            Copiamodiflotti.StartInfo = CopiamodiflottiINFO;
            Copiamodiflotti.Start();
            Copiamodiflotti.WaitForExit();
            progresso.PerformStep();

            MessageBox.Show("Operazione completata.", "Repair FB20");
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            progresso.Value = 0;
            progresso.Maximum = 1;
            FbCommand EneableTriggers = new FbCommand(" update RDB$TRIGGERS set RDB$TRIGGER_INACTIVE=0 where RDB$TRIGGER_NAME in (select A.RDB$TRIGGER_NAME from RDB$TRIGGERS A left join RDB$CHECK_CONSTRAINTS B ON B.RDB$TRIGGER_NAME = A.RDB$TRIGGER_NAME where((A.RDB$SYSTEM_FLAG = 0) or(A.RDB$SYSTEM_FLAG is null)) and(b.rdb$trigger_name is null) AND (NOT(A.RDB$TRIGGER_NAME LIKE 'RDB$%'))); ", ConnectionDbDestino);
            EneableTriggers.ExecuteNonQuery();
            progresso.PerformStep();
            MessageBox.Show("Operazione completata.", "Repair FB20");
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Assicuarti prima di non avere nessun database importante dentro \"C:\\trilogis\" (Verra soprascritto)" );
            MessageBox.Show("Cercare il file FB20 originale per importare i dati.", "Repair FB20");
            OpenFileDialog DatabaseOriginale = new OpenFileDialog();
            DatabaseOriginale.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            DatabaseOriginale.Title = "Scegliere il database originale da importare.";
            DialogResult bDatabaseOriginale = DatabaseOriginale.ShowDialog();
            if (bDatabaseOriginale == DialogResult.OK)
            {
                PercorsoDbOriginale = DatabaseOriginale.FileName;
            }

            if (PercorsoDbOriginale != "")
            {
                try
                {
                    File.Copy(PercorsoDbOriginale, "C:\\trilogis\\trilogis.fb20", true);
                    MessageBox.Show("Avvio trilogis in corso.");
                    Process.Start("C:\\trilogis\\trilogis.exe").WaitForExit();
                    DialogResult result = MessageBox.Show("Aggiornamento database completato? (3logis ha aperto la schermata di utente)","Repair FB20",MessageBoxButtons.YesNo);
                    while (result == DialogResult.No)
                    {
                        Process.Start("C:\\trilogis\\trilogis.exe").WaitForExit();
                        result = MessageBox.Show("Aggiornamento database completato? (Trilogis ha aperto la schermata di utente)", "Repair FB20", MessageBoxButtons.YesNo);
                    }
                    try
                    {
                        if (!Directory.Exists("C:\\DatabaseOriginale"))
                        {
                            Directory.CreateDirectory("C:\\DatabaseOriginale");
                        }
                        File.Copy("C:\\trilogis\\trilogis.fb20", "C:\\DatabaseOriginale\\trilogis.fb20", true);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            if (!Directory.Exists("C:\\DatabaseOriginale"))
                            {
                                Directory.CreateDirectory("C:\\DatabaseOriginale");
                            }
                            File.Copy("C:\\trilogis\\trilogis.fb20", "C:\\DatabaseOriginale\\trilogis.fb20",true);
                        }
                        catch (Exception ex3)
                        {
                            MessageBox.Show("Errore copia database. Copiare MANUALMENTE il file \"C:\\trilogis\\trilogis.fb20\" su \"C:\\DatabaseOriginale\"" + Environment.NewLine + "Errore: " + ex3.ToString());
                        }
                    }
                    MessageBox.Show("Aggiornamento file database originale completato. Spostato in \"C:\\DatabaseOriginale\"");

                    try
                    {
                        File.Copy(Application.StartupPath + "\\trilogis-vuoto.fb20","C:\\trilogis\\trilogis.fb20",true);
                        MessageBox.Show("Copia file database vuoto in C:\\trilogis completata.");
                    }
                    catch (Exception)
                    {
                        try
                        {
                            File.Copy(Application.StartupPath + "\\trilogis-vuoto.fb20", "C:\\trilogis\\trilogis.fb20", true);
                            MessageBox.Show("Copia file database vuoto in C:\\trilogis completata.");
                        }
                        catch (Exception ex3)
                        {
                            MessageBox.Show("Errore copia database. Copiare MANUALMENTE il file " + Application.StartupPath + "\\trilogis-vuoto.fb20\" su \"C:\\trilogis\"" + Environment.NewLine + "Errore: " + ex3.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore avvio database originale. Controllare connessioni aperte. Error: " + ex.ToString());
                }
            }

        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (ConnectionDbDestino.State == ConnectionState.Open)
            {
                ConnectionDbDestino.Dispose();
                System.Diagnostics.Process tricovers = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo tricoversINFO = new System.Diagnostics.ProcessStartInfo();
                tricoversINFO.FileName = @"C:\trilogis\tricovers.exe";
                tricoversINFO.WorkingDirectory = @"C:\trilogis";
                tricovers.StartInfo = tricoversINFO;
                tricovers.Start();
                Environment.Exit(0);
            }
        }
    }
}
