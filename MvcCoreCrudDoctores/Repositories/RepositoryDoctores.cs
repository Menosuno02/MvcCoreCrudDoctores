using MvcCoreCrudDoctores.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcCoreCrudDoctores.Repositories
{
    #region PROCEDIMIENTOS ALMACENADOS
    /*
    CREATE OR ALTER PROCEDURE SP_CREATE_DOCTOR
    (@HOSPITAL_COD INT, @APELLIDO NVARCHAR(50),
    @ESPECIALIDAD NVARCHAR(50), @SALARIO INT)
    AS
        DECLARE @NEXTNO INT
        SELECT @NEXTNO = DOCTOR_NO + 1
        FROM DOCTOR
        INSERT INTO DOCTOR
        VALUES(@HOSPITAL_COD, @NEXTNO, @APELLIDO,
        @ESPECIALIDAD, @SALARIO)
    GO
    */
    #endregion

    public class RepositoryDoctores
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryDoctores()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;User ID=sa;Password=MCSD2023;Encrypt=False";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public async Task<List<Doctor>> GetDoctoresAsync()
        {
            string sql = "SELECT * FROM DOCTOR";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            List<Doctor> doctores = new List<Doctor>();
            while (await this.reader.ReadAsync())
            {
                Doctor doctor = new Doctor();
                doctor.HospitalCod = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                doctor.DoctorNo = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doctor.Apellido = this.reader["APELLIDO"].ToString();
                doctor.Especialidad = this.reader["ESPECIALIDAD"].ToString();
                doctor.Salario = int.Parse(this.reader["SALARIO"].ToString());
                doctores.Add(doctor);
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            return doctores;
        }

        public async Task<Doctor> FindDoctorAsync(int doctorno)
        {
            string sql = "SELECT * FROM DOCTOR WHERE DOCTOR_NO = @ID";
            this.com.Parameters.AddWithValue("@ID", doctorno);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            Doctor doctor = null;
            if (this.reader.ReadAsync() != null)
            {
                doctor = new Doctor();
                doctor.HospitalCod = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                doctor.DoctorNo = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doctor.Apellido = this.reader["APELLIDO"].ToString();
                doctor.Especialidad = this.reader["ESPECIALIDAD"].ToString();
                doctor.Salario = int.Parse(this.reader["SALARIO"].ToString());
            }
            else
            {

            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return doctor;
        }

        public async Task CreateDoctorAsync(int hospitalCod, string apellido, string especialidad, int salario)
        {
            this.com.Parameters.AddWithValue("@HOSPITAL_COD", hospitalCod);
            this.com.Parameters.AddWithValue("@APELLIDO", apellido);
            this.com.Parameters.AddWithValue("@ESPECIALIDAD", especialidad);
            this.com.Parameters.AddWithValue("@SALARIO", salario);
            this.com.CommandText = "SP_CREATE_DOCTOR";
            this.com.CommandType = CommandType.StoredProcedure;
            await this.cn.OpenAsync();
            int result = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task UpdateDoctorAsync(int hospitalCod, int doctorNo, string apellido, string especialidad, int salario)
        {
            string sql = "UPDATE DOCTOR SET HOSPITAL_COD=@HOSPITAL_COD, APELLIDO=@APELLIDO, " +
                "ESPECIALIDAD=@ESPECIALIDAD, SALARIO=@SALARIO WHERE DOCTOR_NO=@DOCTOR_NO";
            this.com.Parameters.AddWithValue("@HOSPITAL_COD", hospitalCod);
            this.com.Parameters.AddWithValue("@DOCTOR_NO", doctorNo);
            this.com.Parameters.AddWithValue("@APELLIDO", apellido);
            this.com.Parameters.AddWithValue("@SALARIO", salario);
            this.com.Parameters.AddWithValue("@ESPECIALIDAD", especialidad);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            await this.cn.OpenAsync();
            int result = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task DeleteDoctorAsync(int doctorno)
        {
            string sql = "DELETE FROM DOCTOR WHERE DOCTOR_NO=@DOCTOR_NO";
            this.com.Parameters.AddWithValue("@DOCTOR_NO", doctorno);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            await this.cn.OpenAsync();
            int result = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}
