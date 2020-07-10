using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BOL;
using IDAL;


namespace DAL
{
    public class DisconnectedDataManager : IManager
    {
        public string conString = ConfigurationManager.ConnectionStrings["localsqlconnection"].ConnectionString;

        //product data access
        public List<Product> GetAllProducts()
        {
            //firstly initialised the list of products
            List<Product> products = new List<Product>();

            //database connectivity
            string query = "SELECT * FROM products";
            //creating objects of sqlconnection and sqlcommand
            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                //using Disconnected ado approach
                //creating sqldataadapter object 
                //and passing Sqlcommand object as argument to it
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //creating dataset object-->contains collection of datarows
                DataSet ds = new DataSet();
                //using fill method of SqlDataAdapter
                //takes the cursor pointerto next row after iterating over currentr row
                da.Fill(ds);
                //ds.Tables[int index]-->gets the data table object at specified index
                //ds.Tables[int index]-->gets the collection of rows that belong to this table
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Product product = new Product();
                    product.Id = int.Parse(row["Id"].ToString());
                    product.Title = row["Title"].ToString();
                    product.Price = int.Parse(row["UnitPrice"].ToString());
                    product.Quantity = int.Parse(row["Quantity"].ToString());

                    //insert the initialised object to collection
                    products.Add(product);
                }

            }
            catch(SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            finally
            {

            }
            return products;

        }
        public Product GetProduct(int id)
        {
            Product product = null;

            string query = "SELECT * FROM products where Id=@product_id";
            //sqlobjects creation
            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(new SqlParameter("@product_id", id));
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                da.Fill(ds);
                //ds.Tables[0]-->get the table from dataset
                DataTable dt = ds.Tables[0];

                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = dt.Columns["Id"];

                DataRow datarow = ds.Tables[0].Rows.Find(id);

                product = new Product();
                product.Id = int.Parse(datarow["Id"].ToString());
                product.Title = datarow["Title"].ToString();
                product.Price = int.Parse(datarow["UnitPrice"].ToString());
                product.Quantity = int.Parse(datarow["Quantity"].ToString());

            }
            catch(SqlException exe)
            {
                throw exe;
            }
            return product;
        }
        public bool InsertProduct(Product product)
        {
            bool status = false;
            string query = "insert into products(Id,Title,UnitPrice,Quantity) values (@product_id,@product_name,@product_price,@product_quantity)";
            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(new SqlParameter("@product_id", product.Id));
            cmd.Parameters.Add(new SqlParameter("@product_name", product.Title));
            cmd.Parameters.Add(new SqlParameter("@product_price", product.Price));
            cmd.Parameters.Add(new SqlParameter("@product_quantity", product.Quantity));
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                SqlCommandBuilder cmbldr = new SqlCommandBuilder(da);
                da.Fill(ds);

                DataRow newRow = ds.Tables[0].NewRow();
                //-->creates a new Data row in table having same schema
                newRow["Id"] = product.Id;
                newRow["Title"] = product.Title;
                newRow["UnitPrice"] = product.Price;
                newRow["Quantity"] = product.Quantity;

                // add new record in database
                ds.Tables[0].Rows.Add(newRow);
                //update changes back to database
                da.Update(ds);
                status = true;

            }
            catch (SqlException exe)
            {
                throw exe;
            }

            return status;
        }
        public bool UpdateProduct(Product product)
        {
            string query = "UPDATE products SET Title = @product_name,UnitPrice=@product_price,Quantity=@product_quantity where Id = @product_id";

            //creating sqlconnection object which takes arguments as connectionstring
            SqlConnection con = new SqlConnection(conString);
            //creating object for sqlcommand which takes query and SqlConnection object
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.Add(new SqlParameter("@product_id", product.Id));
            command.Parameters.Add(new SqlParameter("@product_name", product.Title));
            command.Parameters.Add(new SqlParameter("@product_price", product.Price));
            command.Parameters.Add(new SqlParameter("@product_quantity", product.Quantity));
            //assuming status is false
            bool status = false;
            try
            {

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                SqlCommandBuilder cmbldr = new SqlCommandBuilder(da);

                da.Fill(ds);
               
                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = ds.Tables[0].Columns["Id"];
                //providing primary key
                ds.Tables[0].PrimaryKey = keyColumns;

                DataRow updatingRow = ds.Tables[0].Rows.Find(product.Id);
                updatingRow["Id"] = product.Id;
                updatingRow["Title"] = product.Title;
                updatingRow["UnitPrice"] = product.Price;
                updatingRow["Quantity"] = product.Quantity;

                da.Update(ds);
                status = true;

            }
            catch (SqlException exe)
            {
                throw exe;
            }
            return status;
        }
        public bool DeleteProduct(Product product)
        {
            bool status = false;
            string query = "DELETE FROM products where Id =@product_id";
            SqlConnection con = new SqlConnection(conString);
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@product_id", product.Id);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                SqlCommandBuilder cmbldr = new SqlCommandBuilder(da);
                da.Fill(ds);
                

                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = ds.Tables[0].Columns["Id"];

                DataRow datarow = ds.Tables[0].Rows.Find(product.Id);
                datarow.Delete();
                da.Update(ds);
                status = true;


            }
            catch (SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            return status;
        }

        //userAuthentication access
        public bool RegisterUser(User newUser)
        {
            bool status = false;
            //   newUser.Id = 0;
            //query  string
            string query = "INSERT INTO userauthentication (UserName,Password,ConfirmPassword) VALUES(@user_name,@password,@confirm_password)";

            //creating SqlConnection objects
            SqlConnection con = new SqlConnection(conString);
            //passing query string and  con object
            SqlCommand cmd = new SqlCommand(query, con);
            //passing parameters using SqlCommand object
            //  cmd.Parameters.AddWithValue("@id", newUser.Id + 1);
            cmd.Parameters.Add(new SqlParameter("@user_name", newUser.UserName));
            cmd.Parameters.Add(new SqlParameter("@password", newUser.Password));
            cmd.Parameters.Add(new SqlParameter("@confirm_password", newUser.ConfirmPassword));
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                SqlCommandBuilder cmbldr = new SqlCommandBuilder(da);
                da.Fill(ds);
                /* A DataSet may contain multiple instances of Table. 
                 * ds. Tables[0] is accessing the first table in the Tables collection
                 */
                DataRow newRow = ds.Tables[0].NewRow();
                newRow["UserName"] = newUser.UserName;
                newRow["Password"] = newUser.Password;
                newRow["ConfirmPassword"] = newUser.ConfirmPassword;

                ds.Tables[0].Rows.Add(newRow);
                da.Update(ds);
                status = true;

            }
            catch(SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            return status;
        }
        public bool AuthenticateUser(string usrname, string passwrd)
        {
            User authenticate = null;
            bool status = false;
            string query = "select * from userauthentication where UserName = @usrname and Password = @password";

            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@usrname", usrname);
            cmd.Parameters.AddWithValue("@password", passwrd);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                //??????
                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = ds.Tables[0].Columns["Id"];
                ds.Tables[0].PrimaryKey = keyColumns;

                //finding the row by primary key
                DataRow dataRow = ds.Tables[0].Rows.Find(GetId(usrname, passwrd));

                authenticate = new User();
                authenticate.UserName = dataRow["UserName"].ToString();
                authenticate.Password = dataRow["Password"].ToString();
                authenticate.ConfirmPassword = dataRow["ConfirmPassword"].ToString();
                status = true;

            }
            catch(SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            return status;
        }

        public int GetId(string username,string password)
        {

            User authenticate = null;
            string query = "select Id from userauthentication where UserName = @name and Password= @pass";

            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.Add(new SqlParameter("@name", username));
            cmd.Parameters.Add(new SqlParameter("@pass", password));
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    authenticate = new User();
                    authenticate.Id = int.Parse(reader["Id"].ToString());
                    ////authenticate.UserName = reader["UserName"].ToString();
                    //authenticate.Password = reader["Password"].ToString();
                    //authenticate.ConfirmPassword = reader["ConfirmPassword"].ToString();

                }
            }
            catch(SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            finally
            {
                con.Close();
            }
            return authenticate.Id;
        }

       
    }
}
