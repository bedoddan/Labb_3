using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Labb_3.Models
{
    public class PersonMetoder
    {
        public PersonMetoder()
        {
        }

        public int InsertPerson(Personer p1, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            string sqlstring = "INSERT INTO Personer (Persnr, Namn, Telnr) VALUES (@persnr, @namn, @telnr)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("persnr", SqlDbType.BigInt).Value = p1.Persnr;
            dbCommand.Parameters.Add("namn", SqlDbType.NVarChar, 50).Value = p1.Namn;
            dbCommand.Parameters.Add("telnr", SqlDbType.NVarChar, 15).Value = p1.Telnr;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det skapas ej en person i databasen"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int InsertBete(Beten b1, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            string sqlstring = "INSERT INTO Beten (Typ, Color) VALUES (@typ, @color)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("typ", SqlDbType.NVarChar, 50).Value = b1.Typ;
            dbCommand.Parameters.Add("color", SqlDbType.NVarChar, 50).Value = b1.Color;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det skapas ej ett bete i databasen"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int InsertFisk(Fiskar f1, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            string sqlstring = "INSERT INTO Fiskar (Art, Vikt, Vatten, Persnr, Betenr) VALUES (@art, @vikt, @vatten, @persnr, @betenr)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("art", SqlDbType.NVarChar, 50).Value = f1.Art;
            dbCommand.Parameters.Add("vikt", SqlDbType.Int).Value = f1.Vikt;
            dbCommand.Parameters.Add("vatten", SqlDbType.NVarChar, 50).Value = f1.Vatten;
            dbCommand.Parameters.Add("persnr", SqlDbType.BigInt).Value = f1.Persnr;
            dbCommand.Parameters.Add("betenr", SqlDbType.Int).Value = f1.Betenr;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det skapas ej en person i databasen"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<Totalus> GetFiskarWithDataSet(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "SELECT FiskID, Art, Vikt, Vatten, Namn, Typ, Color FROM Fiskar, Beten, Personer WHERE Beten.Betenr = Fiskar.Betenr and Personer.Persnr = Fiskar.Persnr;";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Totalus> Fisklista = new List<Totalus>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "myFisk");

                int count = 0;
                int i = 0;

                count = myDS.Tables["myFisk"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Totalus pd = new Totalus();

                        pd.ID = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["FiskID"]);
                        pd.Art = myDS.Tables["myFisk"].Rows[i]["Art"].ToString();
                        pd.Vikt = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["Vikt"]);
                        pd.Vatten = myDS.Tables["myFisk"].Rows[i]["Vatten"].ToString();
                        pd.Namn = myDS.Tables["myFisk"].Rows[i]["Namn"].ToString();
                        pd.Typ = myDS.Tables["myFisk"].Rows[i]["Typ"].ToString();
                        pd.Color = myDS.Tables["myFisk"].Rows[i]["Color"].ToString();

                        i++;
                        Fisklista.Add(pd);
                    }
                    errormsg = "";
                    return Fisklista;
                }
                else
                {
                    errormsg = "Det hämtas ingen FISK!";
                    return null;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        //Get Fiskar med 2 in parametrar, används i filtreringen
        public List<Totalus> GetFiskarWithDataSet(out string errormsg, int ValdArt)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "SELECT Fiskar.FiskID, Fiskar.Art, Fiskar.Vikt, Fiskar.Vatten, Personer.Namn, Beten.Typ, Beten.Color FROM Fiskar, Beten, Personer" +
                " WHERE Beten.Betenr = Fiskar.Betenr and Personer.Persnr = Fiskar.Persnr and Fiskar.Art = (SELECT Fiskar.Art FROM Fiskar Where FiskID = @ValdArt);";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("ValdArt", SqlDbType.Int).Value = ValdArt;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Totalus> Fisklista = new List<Totalus>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "myFisk");

                int count = 0;
                int i = 0;

                count = myDS.Tables["myFisk"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Totalus pd = new Totalus();

                        pd.ID = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["FiskID"]);
                        pd.Art = myDS.Tables["myFisk"].Rows[i]["Art"].ToString();
                        pd.Vikt = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["Vikt"]);
                        pd.Vatten = myDS.Tables["myFisk"].Rows[i]["Vatten"].ToString();
                        pd.Namn = myDS.Tables["myFisk"].Rows[i]["Namn"].ToString();
                        pd.Typ = myDS.Tables["myFisk"].Rows[i]["Typ"].ToString();
                        pd.Color = myDS.Tables["myFisk"].Rows[i]["Color"].ToString();

                        i++;
                        Fisklista.Add(pd);
                    }
                    errormsg = "";
                    return Fisklista;
                }
                else
                {
                    errormsg = "Det hämtas ingen FISK!";
                    return null;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<Totalus> Sokning(out string errormsg, string SokString)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "SELECT Fiskar.FiskID, Fiskar.Art, Fiskar.Vikt, Fiskar.Vatten, Personer.Namn, Beten.Typ, Beten.Color FROM Fiskar, Beten, Personer WHERE Beten.Betenr = Fiskar.Betenr and Personer.Persnr = Fiskar.Persnr and Personer.Namn = '@SokString';";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("SokString", SqlDbType.NVarChar).Value = SokString;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Totalus> Fisklista = new List<Totalus>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "myFisk");

                int count = 0;
                int i = 0;

                count = myDS.Tables["myFisk"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Totalus pd = new Totalus();

                        pd.ID = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["FiskID"]);
                        pd.Art = myDS.Tables["myFisk"].Rows[i]["Art"].ToString();
                        pd.Vikt = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["Vikt"]);
                        pd.Vatten = myDS.Tables["myFisk"].Rows[i]["Vatten"].ToString();
                        pd.Namn = myDS.Tables["myFisk"].Rows[i]["Namn"].ToString();
                        pd.Typ = myDS.Tables["myFisk"].Rows[i]["Typ"].ToString();
                        pd.Color = myDS.Tables["myFisk"].Rows[i]["Color"].ToString();

                        i++;
                        Fisklista.Add(pd);
                    }
                    errormsg = "";
                    return Fisklista;
                }
                else
                {
                    errormsg = "Det hämtas ingen FISK!";
                    return null;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        //DEnna används för sökningen
        public List<Totalus> GetFiskarWithDataSet(out string errormsg, String SokString)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "SELECT Fiskar.FiskID, Fiskar.Art, Fiskar.Vikt, Fiskar.Vatten, Personer.Namn, Beten.Typ, Beten.Color FROM Fiskar, Beten, Personer WHERE Beten.Betenr = Fiskar.Betenr and Personer.Persnr = Fiskar.Persnr AND Personer.Namn = @SokString ";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("SokString", SqlDbType.NVarChar).Value = SokString;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Totalus> Fisklista = new List<Totalus>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "myFisk");

                int count = 0;
                int i = 0;

                count = myDS.Tables["myFisk"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Totalus pd = new Totalus();

                        pd.ID = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["FiskID"]);
                        pd.Art = myDS.Tables["myFisk"].Rows[i]["Art"].ToString();
                        pd.Vikt = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["Vikt"]);
                        pd.Vatten = myDS.Tables["myFisk"].Rows[i]["Vatten"].ToString();
                        pd.Namn = myDS.Tables["myFisk"].Rows[i]["Namn"].ToString();
                        pd.Typ = myDS.Tables["myFisk"].Rows[i]["Typ"].ToString();
                        pd.Color = myDS.Tables["myFisk"].Rows[i]["Color"].ToString();

                        i++;
                        Fisklista.Add(pd);
                    }
                    errormsg = "";
                    return Fisklista;
                }
                else
                {
                    errormsg = "Det hämtas ingen FISK!";
                    return null;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        //Sorteringsfunktionen
        public List<Totalus> SorteraNamn(out string errormsg, string sort)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "SELECT Fiskar.FiskID, Fiskar.Art, Fiskar.Vikt, Fiskar.Vatten, Personer.Namn, Beten.Typ, Beten.Color FROM Fiskar, Beten, Personer WHERE Beten.Betenr = Fiskar.Betenr and Personer.Persnr = Fiskar.Persnr ORDER BY Namn";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            //  dbCommand.Parameters.Add("valdSort", SqlDbType.NVarChar).Value = sort;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Totalus> Fisklista = new List<Totalus>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "myFisk");

                int count = 0;
                int i = 0;

                count = myDS.Tables["myFisk"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Totalus pd = new Totalus();

                        pd.ID = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["FiskID"]);
                        pd.Art = myDS.Tables["myFisk"].Rows[i]["Art"].ToString();
                        pd.Vikt = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["Vikt"]);
                        pd.Vatten = myDS.Tables["myFisk"].Rows[i]["Vatten"].ToString();
                        pd.Namn = myDS.Tables["myFisk"].Rows[i]["Namn"].ToString();
                        pd.Typ = myDS.Tables["myFisk"].Rows[i]["Typ"].ToString();
                        pd.Color = myDS.Tables["myFisk"].Rows[i]["Color"].ToString();

                        i++;
                        Fisklista.Add(pd);
                    }
                    errormsg = "";
                    return Fisklista;
                }
                else
                {
                    errormsg = "Det hämtas ingen FISK!";
                    return null;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<Beten> GetBetenWithDataSet(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "SELECT * FROM Beten";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Beten> BetesLista = new List<Beten>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "myFisk");

                int count = 0;
                int i = 0;

                count = myDS.Tables["myFisk"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Beten pd = new Beten();

                        // pd.ID = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["FiskID"]);

                        pd.ID = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["Betenr"]);
                        pd.Typ = myDS.Tables["myFisk"].Rows[i]["Typ"].ToString();
                        pd.Color = myDS.Tables["myFisk"].Rows[i]["Color"].ToString();

                        i++;
                        BetesLista.Add(pd);
                    }
                    errormsg = "";
                    return BetesLista;
                }
                else
                {
                    errormsg = "Det hämtas ingen FISK!";
                    return null;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public List<Personer> GetPersonerWithDataSet(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "SELECT * FROM Personer";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Personer> PersonLista = new List<Personer>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "myFisk");

                int count = 0;
                int i = 0;

                count = myDS.Tables["myFisk"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Personer pd = new Personer();

                        pd.Persnr = Convert.ToInt64(myDS.Tables["myFisk"].Rows[i]["Persnr"]);
                        pd.Namn = myDS.Tables["myFisk"].Rows[i]["Namn"].ToString();
                        pd.Telnr = myDS.Tables["myFisk"].Rows[i]["Telnr"].ToString();

                        i++;
                        PersonLista.Add(pd);
                    }
                    errormsg = "";
                    return PersonLista;
                }
                else
                {
                    errormsg = "Det hämtas ingen Person!";
                    return null;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        //GET Fiskar med Fiskar
        public List<Fiskar> GetFiskarWithFiskModell(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "SELECT * FROM Fiskar";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Fiskar> FiskLista = new List<Fiskar>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "myFisk");

                int count = 0;
                int i = 0;

                count = myDS.Tables["myFisk"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Fiskar f1 = new Fiskar();

                        // pd.ID = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["FiskID"]);

                        f1.ID = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["FiskID"]);
                        f1.Art = myDS.Tables["myFisk"].Rows[i]["Art"].ToString();
                        f1.Vikt = Convert.ToInt32(myDS.Tables["myFisk"].Rows[i]["Vikt"]);
                        f1.Vatten = myDS.Tables["myFisk"].Rows[i]["Vatten"].ToString();

                        i++;
                        FiskLista.Add(f1);
                    }
                    errormsg = "";
                    return FiskLista;
                }
                else
                {
                    errormsg = "Det hämtas ingen Fisk!";
                    return null;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int DeleteFisk(int Fisk_ID, out string errormsg)
        {
            errormsg = "";

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "DELETE FROM FISKAR WHERE FiskID = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = Fisk_ID;

            try
            {
                dbConnection.Open();

                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det skapas ej en person i databasen"; }
                return i;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int DeletePerson(int Person_ID, out string errormsg)
        {
            errormsg = "";

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "DELETE FROM Personer WHERE Persnr = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = Person_ID;

            try
            {
                dbConnection.Open();

                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Kan inte ta bort valda personen, prova annant persnr"; }
                return i;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int DeleteBete(int Bete_ID, out string errormsg)
        {
            errormsg = "";

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            String sqlstring = "DELETE FROM Beten WHERE Betenr = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = Bete_ID;

            try
            {
                dbConnection.Open();

                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Kan inte ta bort valt bete"; }
                return i;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int UpdatePersoner(Personer p1, int Pers_id, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            string sqlstring = "UPDATE Personer SET Persnr = @persnr, Namn = @namn, Telnr = @telnr WHERE Persnr = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("persnr", SqlDbType.NVarChar, 50).Value = p1.Persnr;
            dbCommand.Parameters.Add("namn", SqlDbType.NVarChar, 50).Value = p1.Namn;
            dbCommand.Parameters.Add("telnr", SqlDbType.NVarChar, 50).Value = p1.Telnr;

            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = Pers_id;
            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Gick ej att uppdatera person"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int UpdateFisk(Fiskar p1, int Fisk_id, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            string sqlstring = "UPDATE Fiskar SET Art = @art, Vikt = @vikt, Vatten = @vatten WHERE FiskID = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            //dbCommand.Parameters.Add("fiskid", SqlDbType.NVarChar, 50).Value = p1.ID;
            dbCommand.Parameters.Add("art", SqlDbType.NVarChar, 50).Value = p1.Art;
            dbCommand.Parameters.Add("vikt", SqlDbType.NVarChar, 50).Value = p1.Vikt;
            dbCommand.Parameters.Add("vatten", SqlDbType.NVarChar, 50).Value = p1.Vatten;

            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = Fisk_id;
            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Gick ej att uppdatera Fisk"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int UpdateBeten(Beten p1, int Bete_id, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=FiskDB_Labb3;Integrated Security=True";

            string sqlstring = "UPDATE Beten SET Typ = @typ, Color = @color WHERE Betenr = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("typ", SqlDbType.NVarChar, 50).Value = p1.Typ;
            dbCommand.Parameters.Add("color", SqlDbType.NVarChar, 50).Value = p1.Color;

            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = Bete_id;
            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Gick ej att uppdatera Bete"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}