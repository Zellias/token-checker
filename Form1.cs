using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string freindsList;
        string serversList;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            
           
        }

        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            HttpClient httpClient = new HttpClient();
            HttpClient serversClient = new HttpClient();
            HttpClient friendsClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("authorization", textBox1.Text);
            serversClient.DefaultRequestHeaders.Add("authorization", textBox1.Text);
            friendsClient.DefaultRequestHeaders.Add("authorization", textBox1.Text);

            var endpoint = new Uri("https://discord.com/api/v10/users/@me");
            var endpointServers = new Uri("https://discord.com/api/v10/users/@me/guilds");
            var endpointFriends = new Uri("https://discord.com/api/v10/users/@me/relationships");

            var result = httpClient.GetAsync(endpoint).Result;
            var resultServers = httpClient.GetAsync(endpointServers).Result;
            var resultFriends = httpClient.GetAsync(endpointFriends).Result;

            dynamic jsonServers = JsonConvert.DeserializeObject(resultServers.Content.ReadAsStringAsync().Result);
            dynamic jsonFriends = JsonConvert.DeserializeObject(resultFriends.Content.ReadAsStringAsync().Result);
            dynamic json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result);
           
            pictureBox3.ImageLocation = "";
            pictureBox2.ImageLocation = "";
            label1.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            pictureBox3.ImageLocation = "https://cdn.discordapp.com/banners/" + json.id + "/" + json.banner + ".png?size=1024";
            pictureBox2.ImageLocation = "https://cdn.discordapp.com/avatars/"+ json.id + "/"+json.avatar+".png?size=1024";
            label1.Text = json.username+"#"+json.discriminator;
            richTextBox1.Text = json.bio;
            

            foreach (var item in jsonServers)
            {
                richTextBox2.Text += item.name + '\n' + '\n';
            }
            foreach (var item in jsonFriends)
            {
                richTextBox3.Text += item.user.username  + '\n';
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
          
        }
        public void SaveImage(string imageUrl, string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            HttpClient serversClient = new HttpClient();
            HttpClient friendsClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("authorization", textBox1.Text);
            serversClient.DefaultRequestHeaders.Add("authorization", textBox1.Text);
            friendsClient.DefaultRequestHeaders.Add("authorization", textBox1.Text);

            var endpoint = new Uri("https://discord.com/api/v10/users/@me");
            var endpointServers = new Uri("https://discord.com/api/v10/users/@me/guilds");
            var endpointFriends = new Uri("https://discord.com/api/v10/users/@me/relationships");

            var result = httpClient.GetAsync(endpoint).Result;
            var resultServers = httpClient.GetAsync(endpointServers).Result;
            var resultFriends = httpClient.GetAsync(endpointFriends).Result;

            dynamic jsonServers = JsonConvert.DeserializeObject(resultServers.Content.ReadAsStringAsync().Result);
            dynamic jsonFriends = JsonConvert.DeserializeObject(resultFriends.Content.ReadAsStringAsync().Result);
            dynamic json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result);


            if (!Directory.Exists("./Accounts")) 
            {
                Directory.CreateDirectory("./Accounts");
            }
            if (!Directory.Exists("./Accounts/"+json.username))
            {
                Directory.CreateDirectory("./Accounts/" + json.username);
            }
            if (checkBox1.Checked)
            {

                if (!Directory.Exists("./Accounts/" + json.username + "/Friends-Avatar"))
            {
                Directory.CreateDirectory("./Accounts/" + json.username+ "/Friends-Avatar");
            }
            }

            foreach (var item in jsonServers)
            {
                serversList += item.name +':'+ item.id + '\n' + '\n';
            }
            File.AppendAllText("./Accounts/" + json.username + "/servers.txt", serversList);
            foreach (var item in jsonFriends)
            {
                freindsList += item.user.username + "#"+item.user.discriminator + '\n';
            }
            File.AppendAllText("./Accounts/" + json.username + "/friends.txt", freindsList) ;
            if (checkBox1.Checked)
            {

            foreach (var item in jsonFriends)
                {
                 try
                    {
                        if(item.user.avatar != null)
                        {
                            Console.WriteLine("Done");
                            SaveImage("https://cdn.discordapp.com/avatars/" + item.user.id + "/" + item.user.avatar + ".png?size=1024", "./Accounts/" + json.username + "/Friends-Avatar"+"/"+item.user.id+".png", ImageFormat.Png);
                        }
                    }
                catch (ExternalException)
                    {

                    }
                catch (ArgumentNullException)
                    {

                    }
                }
            }



            try
            {
                SaveImage("https://cdn.discordapp.com/banners/" + json.id + "/" + json.banner + ".png?size=1024", "./Accounts/" + json.username +"/banner.png", ImageFormat.Png);
}
            catch (ExternalException)
            {
                
            }
            catch (ArgumentNullException)
            {
                
            }
            try
            {
                SaveImage("https://cdn.discordapp.com/avatars/" + json.id + "/" + json.avatar + ".png?size=1024", "./Accounts/" + json.username + "/avatar.png", ImageFormat.Png);
            }
            catch (ExternalException)
            {

            }
            catch (ArgumentNullException)
            {

            }
        }
    }
}
