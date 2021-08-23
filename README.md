# Rich Presence for Plex
### Let know your friends what you are watching right now!

![discord_logo](https://i.imgur.com/SC4uRgx.png)   
Rich Presence for Plex is Windows GUI app that will grep your current stream from plex and display it as Rich Presence on your Discord profile.
Unlike other Rich Presences for Plex this doesn't interract with your computer, doesn't scan your PC or web browser. Plex released API for developers, which allows communicate with servers and receiver current streams from your profile, all you need is Discord application ID, Plex Token ID and plex address.  

Application is based on [DiscordRPC by Lachee](https://github.com/Lachee/discord-rpc-csharp). Source code is fully transparent, you can built your own version. Feel free to Fork it and add new features, i will be very happy :)

# Build Dependencies

+ [Newtonsoft.JSON](https://www.newtonsoft.com/json) (DiscordRPC dependency)  
+ [DiscordRPC](https://lachee.github.io/discord-rpc-csharp/docs/)
## Set-Up
Build .exe file via Visual Studio.  
Application will inform you that it has not been found any existing credentials.  
You will need a [discord application](https://discord.com/developers/applications/) and get a ID. 
Also you need a plex token of your account, [plex provides a detailed tutorial](https://support.plex.tv/articles/204059436-finding-an-authentication-token-x-plex-token/) and Plex Direct address, this can be IP of your plex server or .direct url that plex automaticly creates for you.  
Open Settings and write all credentials  

![demo](https://i.imgur.com/esV40fQ_d.webp?maxwidth=760&fidelity=grand)

You can check Run Presence after starting program and click Save. This will create a settings.dat file and next time credentials will be loaded.
Close window and press Run. Application will connect will try to download current streams from plex account. If your credentials are wrong, application will warn you. If the connection was successful, you can see name of the movie or tv show that is currently streamed, or if stream list is empty you can see "Not playing".  
![movie](https://i.imgur.com/BKVuI4H.png) ![tv show](https://i.imgur.com/sxDDkKv.png)


## Common issue

+ No presence - 
Discord RPC doesn't work very well with try catch, so if you will provide wrong application ID or null application will be works but you don't get any presence.

+ Adding image to Discord Presence  -
Since you need to provide a discord application you also need provide a image, you can upload and rewrite image in source code, default name is plex-app-icon

## Other bugs and problems
Thank you for using my application, actually this is my first attempt to do something in C# so i expect many bugs and problems, please open a issue ticket and i will try to fix it. :) Thank you so much

