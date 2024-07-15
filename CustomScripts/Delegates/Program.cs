using System;

namespace EventandDelegates
{
    public class Program 
    {
        static void Main(string[] args)
        {
            var video = new Video() { Title = "Video 1" };
            var videoEncoder = new VideoEncoder();       //发布
            var MailService = new MailService();        //订阅
            var MassageService = new MassageService();        //订阅

            //像方法的指针列表
            videoEncoder.VideoEncoded += MailService.OnVideoEncoded;
            videoEncoder.VideoEncoded += MassageService.OnVideoEncoded;
            //调用前订阅
            videoEncoder.Encode(video);
        }
    }

    //可以位于其他文件夹
    public class MailService
    {
        public void OnVideoEncoded(object source, VideoEventArgs args)
        {
            Console.WriteLine("MailService ");
            
        }
    }
    public class MassageService
    {
        public void OnVideoEncoded(object source, VideoEventArgs args)
        {
            Console.WriteLine("MassageService ");
            
        }
    }
}