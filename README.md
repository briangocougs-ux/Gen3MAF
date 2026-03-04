# Gen3MAF
I created this utility to aid in the tuning of the MAF curve on a GEN 3 LS, in particular the LQ4 in my 2003 GMC Yukon Denali.

I am a novice to the tuning world. The only reason I bought the tuner hardware was to enable the e-fans in my vehicle when coverted from mechanical.
Since I paid $400 for the product, I decided to see what else I could do with it, so I started playing with tuning the MAF and VE tables. In the process I felt like I would like more systemmatic way of managing the correction dat and how it is applied to create a new MAF curve.

I am guessing the audience for this app is mostly people like me. I'm think a profession tuner will probably not have so much use for this.

The app logically fits in between the workflow between the tuner editor and the tuner scanning app. You input the current MAF curve of your vehicle and the feaback back data from the scanner histogram data of the STFT or AFR err.

In the app you can look look at the resulting new MAF curve graphically and apply adjustment as you see fit. The app can can optionally linearly interpret between buckets missing correction data. You can adjust the what percentage of the correction is applied to the existing curve. You can set a threshold that that must be exceeded to have the correction applied. You can also restrict the range of freaquencies that the correction is applied to.

Once you have adjusted the correction as you see fit, You can copy the new curve from the app and paste it in the tune editor.

The app records each cycle of of generating a new curve from the scanner correction data. You can later review each one.

The application also supports two modes of processing feadback data. A single bucket mode where it applies a single bucket of histogram correction data to each frequency in the curve. It also supports generating three histogram buckets of correction data for each frequency in the curve. The goal of using three bucket is reduce noise from the correction data.

The app is pretty simple and utititarian. Hopefully the workflow it pretty obvious.
