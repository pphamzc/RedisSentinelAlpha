using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceStack.Redis;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Text;
using ServiceStack.DataAnnotations;
using SimpleInjector;

namespace RedisSentinelAlpha
{
    public partial class Form1 : Form
    {

        IRedisClientsManager localRedisManager;
        public Form1(IRedisClientsManager redisManager)
        {
            localRedisManager = redisManager;
            InitializeComponent();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnectSent_Click(object sender, EventArgs e)
        {
            ConnectToSentinel(null);
        }

        private void ConnectToSentinel(string conx)
        {
            try
            {

                txbOutput.Text = "Start Sentinel : " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine;


                var pooledCon = localRedisManager;
                //txbOutput.Text += "Sentinel Master = " + client. + Environment.NewLine;
                txbOutput.Text += "Start Sentinel Done : " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine;

                Dictionary<string, string> stats = (pooledCon as PooledRedisClientManager).GetStats();
                //txbOutput.Text += "Pooled Connections stats : " +  + Environment.NewLine;   

                using (IRedisClient client = pooledCon.GetClient())
                {
                    txbOutput.Text += "Use Main/Write Connection : " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine;
                    txbOutput.Text += "Start Write : " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine;
                    client.Set<string>("Food", textBox1.Text, DateTime.UtcNow.AddSeconds(21600));
                    client.Add<string>("Food_Add", textBox1.Text, DateTime.UtcNow.AddSeconds(21600));
                    txbOutput.Text += "Setting Food=" + textBox1.Text + Environment.NewLine;
                   
                }

                using (IRedisClient client = pooledCon.GetReadOnlyClient())
                {
                    txbOutput.Text += "Use Read Only Connection : " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine;
                    txbOutput.Text += "Getting Food" + Environment.NewLine;
                    txbOutput.Text += client.Get<string>("Food") + Environment.NewLine;
                }

                txbOutput.Text += "End Write : " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
