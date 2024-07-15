using System;
using System.Threading;

namespace EventandDelegates
{
    
    public class VideoEventArgs: EventArgs
    {
        public Video Video { get; set; }  
    }
    
    public class VideoEncoder
    {
        //1 - define delegate
        //2 - define an event based on that delegate
        //3 - Raise the event

        
        //1 - 决定了订阅的形式
        public delegate void VideoEncodeEventHandler(object source, VideoEventArgs args);
        //2 - 创建事件
        public event VideoEncodeEventHandler VideoEncoded;

        public void Encode(Video video)
        {
            Console.WriteLine("Encoding Video...");
            Thread.Sleep(2000);

            //3 - 发起事件 - 通知所有订阅者
            OnVideoEncode(video);
        }

        protected virtual void OnVideoEncode(Video video)
        {
            if (VideoEncoded != null)
                VideoEncoded(this, new VideoEventArgs(){Video = video});
        }
    }
}