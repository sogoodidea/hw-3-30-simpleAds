using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;

namespace hw_3_30b.data
{
    public class SimpleAdDb
    {
        private string _conStr = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAdProject;Integrated Security=True;";

        public List<Post> GetPosts()
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Posts ORDER BY DateCreated DESC";
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Post> posts = new List<Post>();
                while (reader.Read())
                {
                    Post newPost = new Post
                    {
                        DateCreated = (DateTime)reader["DateCreated"],
                        Text = (string)reader["Text"],
                        Name = reader.GetOrNull<string>("Name"),
                        PhoneNumber = (string)reader["PhoneNumber"],
                        Id = (int)reader["Id"]
                    };
                    posts.Add(newPost);
                }
                return posts;
            }
        }

        public void AddPost(Post post)
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Posts(Name, PhoneNumber, Text, DateCreated)
                                    VALUES(@name, @phonenumber, @text, GETDATE())
                                    SELECT SCOPE_IDENTITY()";
                object name = post.Name;
                if (name == null)
                {
                    name = DBNull.Value;
                }
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@phonenumber", post.PhoneNumber);
                cmd.Parameters.AddWithValue("@text", post.Text);
                conn.Open();
                post.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }
        public void DeleteAd(int postId)
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Posts WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", postId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
