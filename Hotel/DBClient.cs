using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel
{
    public class DBClient
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotel;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private int GetMaxFacilityID(SqlConnection connection)
        {
            Console.WriteLine("Calling -> GetMaxFacilityID");

            //This SQL command will fetch one row from the Facility table: The one with the max Facility_ID
            string queryStringMaxFacilityID = "SELECT  MAX(Facility_ID)  FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringMaxFacilityID}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringMaxFacilityID, connection);
            SqlDataReader reader = command.ExecuteReader();

            //Assume undefined value 0 for max MaxFacility_ID
            int MaxFacility_ID = 0;

            //Are there any rows in the query
            if (reader.Read())
            {
                //Yes, get max Facility_ID
                MaxFacility_ID = reader.GetInt32(0); //Reading int from 1st column
            }

            //Close reader
            reader.Close();

            Console.WriteLine($"Max Facility#: {MaxFacility_ID}");
            Console.WriteLine();

            //Return max Facility_ID
            return MaxFacility_ID;
        }

        private List<Facility> ListAllFacilities(SqlConnection connection)
        {
            Console.WriteLine("Calling -> ListAllFacilities");

            //This SQL command will fetch all rows and columns from the Facility table
            string queryStringAllFacilities = "SELECT * FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringAllFacilities}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringAllFacilities, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Listing all Facilities:");

            //NO rows in the query 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No Facilities in database");
                reader.Close();

                //Return null for 'no Facility found'
                return null;
            }

            //Create list of Facilities found
            List<Facility> Facility = new List<Facility>();
            while (reader.Read())
            {
                //If we reached here, there is still one Facility to be put into the list 
                Facility nextFacility = new Facility()
                {
                    Facility_ID = reader.GetInt32(0), //Reading int from 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column
                };

                //Add Facility to list
                Facility.Add(nextFacility);

                Console.WriteLine(nextFacility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return list of Facilities
            return Facility;
        }

        private int InsertFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> InsertFacility");

            //This SQL command will insert one row into the Facility table with primary key Facility_ID
            string insertCommandString = $"INSERT INTO Facility VALUES({facility.Facility_ID}, '{facility.Name}')";
            Console.WriteLine($"SQL applied: {insertCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(insertCommandString, connection);

            Console.WriteLine($"Creating Facility #{facility.Facility_ID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected 
            return numberOfRowsAffected;
        }


        private int UpdateFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> UpdateFacility");

            //This SQL command will update one row from the Facility table: The one with primary key Facility_ID
            string updateCommandString = $"UPDATE Facility SET Name='{facility.Name}' WHERE Facility_ID = {facility.Facility_ID}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(updateCommandString, connection);
            Console.WriteLine($"Updating Facility #{facility.Facility_ID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }


        private int DeleteFacility(SqlConnection connection, int facility_ID)
        {
            Console.WriteLine("Calling -> DeleteFacility");

            //This SQL command will delete one row from the Facility table: The one with primary key Facility_ID
            string deleteCommandString = $"DELETE FROM Facility WHERE Facility_ID = {facility_ID}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            Console.WriteLine($"Deleting Facility #{facility_ID}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private Facility GetFacility(SqlConnection connection, int facility_ID)
        {
            Console.WriteLine("Calling -> GetFacility");

            //This SQL command will fetch the row with primary key Facility_ID from the Facility table
            string queryStringOneHotel = $"SELECT * FROM Facility WHERE Facility_ID = {facility_ID}";
            Console.WriteLine($"SQL applied: {queryStringOneHotel}");

            //Prepare SQl command
            SqlCommand command = new SqlCommand(queryStringOneHotel, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine($"Finding Facility#: {facility_ID}");

            //NO rows in the query? 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No Facility in database");
                reader.Close();

                //Return null for 'no Facility found'
                return null;
            }

            //Fetch Facility object from the database
            Facility facility = null;
            if (reader.Read())
            {
                facility = new Facility()
                {
                    Facility_ID = reader.GetInt32(0), //Reading int fro 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column
                };

                Console.WriteLine(facility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return found Facility
            return facility;
        }

        public void Start()
        {
            //Apply 'using' to connection (SqlConnection) in order to call Dispose (interface IDisposable) 
            //whenever the 'using' block exits
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open connection
                connection.Open();

                //List all Facilities in the database
                ListAllFacilities(connection);

                //Create a new Facility with primary key equal to current max primary key plus 1
                Facility newFacility = new Facility()
                {
                    Facility_ID = GetMaxFacilityID(connection) + 1,
                    Name = "Facility x",
                };

                //Insert the Facility into the database
                InsertFacility(connection, newFacility);

                //List all Facilities including the the newly inserted one
                ListAllFacilities(connection);

                //Get the newly inserted Facility from the database in order to update it 
                Facility FacilityToBeUpdated = GetFacility(connection, newFacility.Facility_ID);

                //Alter Name properties
                
                FacilityToBeUpdated.Name += "(updated)";

                //Update the Facility in the database 
                UpdateFacility(connection, FacilityToBeUpdated);

                //List all Facilities including the updated one
                ListAllFacilities(connection);

                //Get the updated Facility in order to delete it
                Facility FacilityToBeDeleted = GetFacility(connection, FacilityToBeUpdated.Facility_ID);

                //Delete the Facility
                DeleteFacility(connection, FacilityToBeDeleted.Facility_ID);

                //List all Facilities - now without the deleted one
                ListAllFacilities(connection);
            }
        }
    }
}
