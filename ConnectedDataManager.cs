using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using BOL;
using IDAL;

namespace DAL
{
    public class ConnectedDataManager:IManager
    {
        /* this class is for crud operation
        */
        //intitialized the connection string as static variable
        //it contains 3 things 1.who is provider
        //2.what is the name of database
        //3.where is the location of database
        public string conString = ConfigurationManager.ConnectionStrings["localsqlconnection"].ConnectionString;
        /*   for products */
        //1. retrieving all data
        public  List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            //Database connectivity
            string query = "SELECT * FROM products";

            //creating sqlconnection object
            SqlConnection con = new SqlConnection(conString);
            SqlCommand command = new SqlCommand(query, con);
            try
            {
                //openning the connection
                con.Open();

                //this will read row by row and execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    //needed to initialise the attributes
                    Product product = new Product();
                    product.Id = int.Parse(reader["Id"].ToString());
                    product.Title = reader["Title"].ToString();
                    product.Quantity = int.Parse(reader["Quantity"].ToString());
                    product.Price = int.Parse(reader["UnitPrice"].ToString());

                    //once intialised now put the object into collection
                    products.Add(product);
                }

            }
            catch (SqlException exe)
            {
                Console.WriteLine(exe.Message);

            }
            finally
            {
                //closing the resource in finally block
                con.Close();
            }
            //returning the list
            return products;

        }
        //2.retrieving single object details
        public  Product GetProduct(int id)
        {
            Product product = null;
            string query = "SELECT * FROM products where Id=@product_id";

            //creating sqlconnection object
            SqlConnection con = new SqlConnection(conString);
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.Add(new SqlParameter("@product_id", id));
            try
            {
                //openning the connection
                con.Open();
                //advances the SqlDataReader to the next record
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product();
                    product.Id = int.Parse(reader["Id"].ToString());
                    product.Title = reader["Title"].ToString();
                    product.Quantity = int.Parse(reader["Quantity"].ToString());
                    product.Price = int.Parse(reader["UnitPrice"].ToString());
                }

            }
            catch (SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            finally
            {
                con.Close();
            }
            return product;
        }
        //3.updating the object of product
        public  bool UpdateProduct(Product product)
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
                //openning the connection
                con.Open();
                //use execute non query for delete update insert
                int rows_affected = command.ExecuteNonQuery();
                if (rows_affected > 0)
                {
                    status = true;
                }

            }
            catch (SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            finally
            {
                //closing the resources openned
                con.Close();
            }
            return status;
        }
        //4.insert a new object in database
        public  bool InsertProduct(Product product)
        {
            bool status = false;
            string query = "insert into products(Id,Title,UnitPrice,Quantity) values (@product_id,@product_name,@product_price,@product_quantity)";
            SqlConnection con = new SqlConnection(conString);
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.Add(new SqlParameter("@product_id", product.Id));
            command.Parameters.Add(new SqlParameter("@product_name", product.Title));
            command.Parameters.Add(new SqlParameter("@product_price", product.Price));
            command.Parameters.Add(new SqlParameter("@product_quantity", product.Quantity));
            try
            {
                con.Open();
                int rowAffected = command.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    status = true;
                }

            }
            catch (SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            finally
            {
                con.Close();
            }

            return status;
        }
        //5.Delete object
        public  bool DeleteProduct(Product product)
        {
            bool status = false;
            string query = "DELETE FROM products where Id =@product_id";
            SqlConnection con = new SqlConnection(conString);
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@product_id", product.Id);
            //command.Parameters.Add(new SqlParameter("@product_id", product.Id));
            try
            {
                con.Open();
                int rowAffetced = command.ExecuteNonQuery();
                if (rowAffetced > 0)
                {
                    status = true;
                }

            }
            catch (SqlException exe)
            {
                Console.WriteLine(exe.Message);

            }
            finally
            {
                con.Close();

            }

            return status;
        }

        /*   for userauthentication table for RegisterViewModel */
        ///1.registeration logic
        public  bool RegisterUser(User newUser)
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
                //openning the connection
                con.Open();
                int rowaffected = cmd.ExecuteNonQuery();
                if (rowaffected > 0)
                {
                    status = true;
                }
            }
            catch (SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            finally
            {
                //closing the resources
                con.Close();
            }

            return status;

        }

        //2.login logic to get object
        public  bool AuthenticateUser(string usrname, string passwrd)
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
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //initialising the attributes
                    authenticate = new User();
                    authenticate.UserName = reader["UserName"].ToString();
                    authenticate.Password = reader["Password"].ToString();
                    authenticate.ConfirmPassword = reader["ConfirmPassword"].ToString();
                    status = true;
                }

            }
            catch (SqlException exe)
            {
                Console.WriteLine(exe.Message);
            }
            finally
            {
                con.Close();
            }
            return status;
        }
    }
}
