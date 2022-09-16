using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace Alfasoft_Challenge.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ContactInfo> listContacts = new List<ContactInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=UsersDB;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM contacts";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ContactInfo contactInfo = new ContactInfo();
                                contactInfo.id = "" + reader.GetInt32(0);
                                contactInfo.nome = reader.GetString(1);
                                contactInfo.contact = reader.GetString(2);
                                contactInfo.email = reader.GetString(3);

                                listContacts.Add(contactInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class ContactInfo
    {
        public string id;
        public string nome;
        public string contact;
        public string email;
    }
}
