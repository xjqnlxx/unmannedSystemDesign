using UnityEngine;
using System.Collections;
//引入库
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
 
public class Server : MonoBehaviour
{
    //以下默认都是私有的成员
    Socket 服务器端socket; //服务器端socket
    Socket 客户端socket; //客户端socket
    IPEndPoint 侦听端口; //侦听端口
    string 接收的字符串; //接收的字符串
    string 发送的字符串; //发送的字符串
    byte[] 接收的数据=new byte[1024]; //接收的数据，必须为字节
    byte[] 发送的数据=new byte[1024]; //发送的数据，必须为字节
    int 接收的数据长度; //接收的数据长度
    Thread 连接线程; //连接线程
 
    //初始化
    void InitSocket()
    {
        //定义侦听端口,侦听任何IP
        侦听端口=new IPEndPoint(IPAddress.Any,8081);
        //定义套接字类型,在主线程中定义
        服务器端socket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //连接
        服务器端socket.Bind(侦听端口);
        //开始侦听,最大10个连接
        服务器端socket.Listen(10);
 
        //开启一个线程连接，必须的，否则主线程卡死
        连接线程=new Thread(new ThreadStart(SocketReceive));
        连接线程.Start();
    }
 
    //连接
    void SocketConnect()
    {
        if(客户端socket!=null)
            客户端socket.Close();
        //控制台输出侦听状态
        print("等待一个用户端的链接\n");
        //一旦接受连接，创建一个客户端
        客户端socket=服务器端socket.Accept();
        //获取客户端的IP和端口
        IPEndPoint ipEndClient=(IPEndPoint)客户端socket.RemoteEndPoint;
        //输出客户端的IP和端口
        print(ipEndClient.Address.ToString()+":"+ipEndClient.Port.ToString()+"链接到了这台服务器\n");
        //连接成功则发送数据
        发送的字符串="欢迎链接到服务器端.\n";
        SocketSend(发送的字符串);
    }
 
    void SocketSend(string 要发送的字符串)
    {
        //清空发送缓存
        发送的数据=new byte[1024];
        //数据类型转换
        发送的数据=Encoding.UTF8.GetBytes(要发送的字符串);
        //发送
        客户端socket.Send(发送的数据,发送的数据.Length,SocketFlags.None);
    }
 
    //服务器接收
    void SocketReceive()
    {
        //连接
        SocketConnect();      
        //进入接收循环
        while(true)
        {
            //对data清零
            接收的数据=new byte[1024];
            //获取收到的数据的长度
            接收的数据长度=客户端socket.Receive(接收的数据);
            //如果收到的数据长度为0，则重连并进入下一个循环
            if(接收的数据长度==0)
            {
                SocketConnect();
                continue;
            }
            //输出接收到的数据
            接收的字符串=Encoding.UTF8.GetString(接收的数据,0,接收的数据长度);
            print(接收的字符串);
            //将接收到的数据经过处理再发送出去
            发送的字符串=接收的字符串;
            SocketSend(发送的字符串);
        }
    }
 
    //连接关闭
    void SocketQuit()
    {
        //先关闭客户端
        if(客户端socket!=null)
            客户端socket.Close();
        //再关闭线程
        if(连接线程!=null)
        {
            连接线程.Interrupt();
            连接线程.Abort();
        }
        //最后关闭服务器
        服务器端socket.Close();
        print("链接关闭");
    }
 
    // Use this for initialization
    void Start()
    {
        InitSocket(); //在这里初始化server
    }
 
 
    // Update is called once per frame
    void Update()
    {
 
    }
 
    void OnApplicationQuit()
    {
        SocketQuit();
    }
}
