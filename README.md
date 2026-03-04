# Gen3MAF
I created this utility to aid in the tuning of the MAF curve on a GEN 3 LS, in particular the LQ4 in my 2003 GMC Yukon Denali.

I am a novice to the tuning world. The only reason I bought the tuner hardware was to enable the e-fans in my vehicle when coverted from mechanical.
Since I paid $400 for the product, I decided to see what else I could do with it, so I started playing with tuning the MAF and VE tables. In the process I felt like I would like more systemmatic way of managing the correction data and how it is applied to create a new MAF curve.

I am guessing the audience for this app is mostly people like me. I'm thinking a profession tuner will probably not have so much use for this.

The app logically fits in between the workflow between the tuner editor and the tuner scanning app. You input the current MAF curve of your vehicle and the feaback back data from the scanner histogram data of the STFT or AFR err.

In the app you can look at the resulting new MAF curve graphically and apply adjustment as you see fit. The app can can optionally linearly interpolate between buckets missing correction data. You can adjust the what percentage of the correction is applied to the existing curve. You can set a threshold that that must be exceeded to have the correction applied. You can also restrict the range of freaquencies that the correction is applied to.

Once you have adjusted the correction as you see fit, You can copy the new curve from the app and paste it in the tune editor.

The app records each cycle of of generating a new curve from the scanner correction data. You can later review each one.

The application also supports two modes of processing feadback data. A single bucket mode where it applies a single bucket of histogram correction data to each frequency in the curve. This is like how you would do it now, except the bucket is aligned over the frequency. It also supports generating three histogram buckets of correction data for each frequency in the curve. The goal of using three bucket is reduce noise from the correction data.

The app is pretty simple and utititarian. Hopefully the workflow it pretty obvious.

The app has a typical new, open close save menu. To Start select NEW.

<img width="536" height="247" alt="image" src="https://github.com/user-attachments/assets/0b822d51-43ae-45c7-adb2-6d657fbddfb3" />

After that you will get the New vehicle dialog.

<img width="801" height="487" alt="image" src="https://github.com/user-attachments/assets/380697ee-0239-41a2-82af-4fac9e4b5879" />

Vehicle name is required, ECU and OS are optional. You can then pick the start and end frequencies for the MAF curve. You also specify the step between.The default values are for the MAF in my GMT800 SUV.

After that you choose how many buckets you wnat. Signle buck most closely resembles what you normally do with the tuning editor and scanner. The one differenct it the bucket it centered on the frequency instead of after it. 

With triple bucket there is one bucket centered on the target frequecy and one 1/3 of the distance to the ajoining frequency targets. When processing the correction data the app tries to pick an average value for the final correction value. This was really my whole motivation for writing the app.

Now you create a new Tune cycle.

<img width="478" height="182" alt="image" src="https://github.com/user-attachments/assets/f1e8b4ab-77c5-4438-bdf5-c11a493af95f" />


With this you begin the process.

<img width="1206" height="287" alt="image" src="https://github.com/user-attachments/assets/18c30ede-7f15-4378-8f99-f50e04b01e77" />

The top TextBox contains bucket data that you would paste into the scanner histgram configuration.

<img width="703" height="148" alt="image" src="https://github.com/user-attachments/assets/957a1b3a-8eac-4587-8849-b9cb26b15afa" />

In the next TextBox is where you paste the airflow for your current MAF curve from the editor app. Copy it like this.

<img width="677" height="305" alt="image" src="https://github.com/user-attachments/assets/948dc28b-8e7f-4ec6-91ec-31cb537f0f8d" />

Then right click and paste.

<img width="562" height="142" alt="image" src="https://github.com/user-attachments/assets/1bf777eb-5987-4f05-877c-ec7037feff24" />

Once you have pasted the info, press the button to process it. The GridView below will fill out with the MAF curve. You can verify with what is on the editor app.


<img width="941" height="265" alt="image" src="https://github.com/user-attachments/assets/b48f16ad-0e8a-4580-818d-6c1805a7f616" />

At this point you can pause your work and come back to it later when you have collected the correction data from a drive. Lets assume you already have it.

Go to the scanner app and copy it like this.

<img width="1207" height="247" alt="image" src="https://github.com/user-attachments/assets/b00a734d-9a3a-492c-ba85-739fb519f666" />

Once you have paste it into the next TextBox.


<img width="763" height="147" alt="image" src="https://github.com/user-attachments/assets/64e968a7-fc5e-4812-a64f-118e2c752ae8" />

The TextBox will likely be empty becuase there are alot of tabs with no data to be displayed, but a scrol bar will appear and you can scroll to verify that there is somehting there. Now press the button to Apply the adjustments. The GridView below will fill out.




<img width="1208" height="402" alt="image" src="https://github.com/user-attachments/assets/7d77a417-f629-4911-938e-46ba126f82f6" />

Now the correction data has been processed. You can adjust things at this point as you desire. 
1. You can reduce the correction that is applied. It starts at 100%. You may want to reduce it so the change is less likely to overshoot the goal airflow.
2. You can set a minimum threshold for the correction to exceed before it is applied. This can be used to reduce the oscillation around the ideal airflow.
3. You can have the program interpolate between buckets with no correction data. If it is for the initial correction from stock, this is probably fine. For more fine adjustments you may want to turn it off.
4. You can constrain the range of frequecies that the correction data will be applied to. For example with you are tunning WOT P.E stuff and you have a bunch of random correction data at low airflow rates, you can exclude those.

You can plot the adjustment data by pressing the button.


<img width="1162" height="612" alt="image" src="https://github.com/user-attachments/assets/11d504cf-b34b-4a1d-a001-d78674aa21ee" />

You can also plot all the raw data.

<img width="1152" height="617" alt="image" src="https://github.com/user-attachments/assets/850e1546-9d3c-459b-8415-53c0ca4321c9" />

You can zoom in by left clicking the mouse and drawing a rectangle.

<img width="1160" height="615" alt="image" src="https://github.com/user-attachments/assets/206ccb36-a49d-4500-a33e-5de86003ef8c" />

The blue line it the original MAF curve, The yellow is the curve with the correction data applied. The diamond markers are the actual frequency points from the MAF curve. The small marker are the extra tuning points added for the triple buckets.

Once you are happy with the changes, right click and select copy. You now paste this back into the tune editor.

<img width="1203" height="236" alt="image" src="https://github.com/user-attachments/assets/da706655-3505-41f4-8699-4adf619f80ea" />

You can now complete the tune cycle and save it, or just discard it.





