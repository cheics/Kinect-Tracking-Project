clc;

basePath='C:\Users\Ameen Jamil\Desktop\SYDE461\cheics-Kinect-Tracking-Project-fc2e9bc';
basePath2=strcat(basePath,'\DataFiles\Experiment1\');

headOn_dataIn_G1=importdata(strcat(basePath2,'headOnGood.csv'));
headOn_dataIn_B1=importdata(strcat(basePath2,'headOnBadBack.csv'));
headOn_dataIn_B2=importdata(strcat(basePath2,'headOnBadHip.csv'));

headOn_ts_G1=horzcat(40:70,175:195,290:310,405:435);
headOn_ts_B1=horzcat(30:70,145:165,253:265,354:380,465:480);
headOn_ts_B2=horzcat(50:60,165:170,273:278,376:383,485:495);

%plot(headOn_dataIn_G1.data(:,53));

% ff_dataIn_G1=importdata(strcat(basePath2,'45Good.csv'));
% ff_dataIn_B1=importdata(strcat(basePath2,'45BadBack.csv'));
% ff_dataIn_B2=importdata(strcat(basePath2,'45BadHip.csv'));
% 
% 
cc=GetFeatures(headOn_dataIn_G1.data, headOn_ts_G1);
dd=GetFeatures(headOn_dataIn_G1.data, headOn_ts_B1);
ee=GetFeatures(headOn_dataIn_B2.data, headOn_ts_B2);
% 
% 
makePlots(cc, dd, ee);
score = clustering2(cc, dd, ee);

% plot3(cc(1,:), cc(2,:), cc(3,:), 'go', ...
%     dd(1,:), dd(2,:), dd(3,:), 'ro', ...
%     ee(1,:), ee(2,:), ee(3,:), 'bo');


