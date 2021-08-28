# Rich Presence for Plex
### Let know your friends what you are watching right now!

![discord_logo](https://i.imgur.com/SC4uRgx.png)   
Rich Presence for Plex is Windows GUI app that will grep your current stream from plex and display it as Rich Presence on your Discord profile.
Unlike other Rich Presences for Plex this doesn't interract with your computer, doesn't scan your PC or web browser. Plex released API for developers, which allows communicate with servers and receiver current streams from your profile. I was inspirated from [Ombrelin's Rich Presence](https://github.com/Ombrelin/plex-rich-presence) in Java but I wanted to make lightway native Windows version since i'm not big fan of Java and i wanted to learn C#.

Application is based on [DiscordRPC by Lachee](https://github.com/Lachee/discord-rpc-csharp). Source code is fully transparent, you can built your own version. Feel free to Fork it and add new features, i will be very happy :)

# Build Dependencies

+ [Newtonsoft.JSON](https://www.newtonsoft.com/json) (DiscordRPC dependency)  
+ [DiscordRPC](https://lachee.github.io/discord-rpc-csharp/docs/)
## Set-Up
Build .exe file via Visual Studio.  
For first time App will generate your pin and code will open your native web browser with plex address. You need to sign in to your profile. Default countdown is 15 seconds. Then application automatically store credentials for next usage and run presence. Of course PMS URI is downloaded. If the connection was successful, you can see name of the movie or tv show that is currently streamed, or if stream list is empty you can see "Not playing".  

![gui_inactive](https://i.imgur.com/aGHtF7x.png) ![gui_active](https://i.imgur.com/S21VTDW.png)


# Example of Rich Presence

![movie](https://i.imgur.com/BKVuI4H.png) ![tv show](https://i.imgur.com/sxDDkKv.png)



## Other bugs and problems
Thank you for using my application, actually this is my first attempt to do something in C# so i expect many bugs and problems, please open a issue ticket and i will try to fix it. :) Thank you so much

