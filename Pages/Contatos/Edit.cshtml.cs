using Alfasoft_Challenge.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Alfasoft_Challenge.Pages.Contatos
{
    public class EditModel : PageModel
    {
        public ContactInfo contatoInfo = new ContactInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UsersDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM contacts WHERE id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", contatoInfo.id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ContactInfo contactInfo = new ContactInfo();
                                contactInfo.id = "" + reader.GetInt32(0);
                                contactInfo.nome = reader.GetString(1);
                                contactInfo.contact = reader.GetString(2);
                                contactInfo.email = reader.GetString(3);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            contatoInfo.nome = Request.Form["nome"];
            contatoInfo.contact = Request.Form["contact"];
            contatoInfo.email = Request.Form["email"];

            if (contatoInfo.nome.Length <= 5)
            {
                errorMessage = "O nome deve ter mais de 5 lettras!";
            }
            if (contatoInfo.contact.Length != 9)
            {
                errorMessage = "O contato deve ter 9 digitos!";
            }
            if (!contatoInfo.email.Contains("@"))
            {
                errorMessage = "O email não est valido!";
            }

            try
            {
                String connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UsersDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO contacts " +
                                 "(nome, contact, email) VALUES" +
                                 "(@nome, @contact, @email);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nome", contatoInfo.nome);
                        command.Parameters.AddWithValue("@contact", contatoInfo.contact);
                        command.Parameters.AddWithValue("@email", contatoInfo.email);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            contatoInfo.nome = ""; contatoInfo.contact = ""; contatoInfo.email = "";
            successMessage = "Novo contato cadastrade com sucesso!";

            Response.Redirect("/Contatos/Index");
        }
    }
}
