basePath='C:\Users\cheics\Documents\Visual Studio 2010\Projects\';
basePath2=strcat(basePath,'Beta2 Samples\Kinect-Tracking-Project\DataFiles\Experiment1\');

reps_in_data=5;
windowData=0;
toKeep=1;

multiplyData=0;
if multiplyData~=0
    windowData=5;
    toKeep=0.5;
end

headOn_dataIn_G1=importdata(strcat(basePath2,'headOnGood.csv'));
headOn_dataIn_B1=importdata(strcat(basePath2,'headOnBadBack.csv'));
headOn_dataIn_B2=importdata(strcat(basePath2,'headOnBadHip.csv'));

ff_dataIn_G1=importdata(strcat(basePath2,'45Good.csv'));
ff_dataIn_B1=importdata(strcat(basePath2,'45BadBack.csv'));
ff_dataIn_B2=importdata(strcat(basePath2,'45BadHip.csv'));

pp_dataIn_G1=importdata(strcat(basePath2,'profileGood.csv'));
pp_dataIn_B1=importdata(strcat(basePath2,'profileBadBack.csv'));
pp_dataIn_B2=importdata(strcat(basePath2,'profileBadHip.csv'));

headOn_ts_G1=poseFinder(headOn_dataIn_G1.data, ...
    reps_in_data, windowData, toKeep);
headOn_ts_B1=poseFinder(headOn_dataIn_B1.data, ...
    reps_in_data, windowData, toKeep);
headOn_ts_B2=poseFinder(headOn_dataIn_B2.data, ...
    reps_in_data, windowData, toKeep);

% ff_ts_G1=poseFinder(ff_dataIn_G1.data(:,53), ...
%     5, windowData, toKeep);
% ff_ts_B1=poseFinder(ff_dataIn_B1.data(:,53), ...
%     5, windowData, toKeep);
% ff_ts_B2=poseFinder(ff_dataIn_B2.data(:,53), ...
%     5, windowData, toKeep);
% 
% pp_ts_G1=poseFinder(pp_dataIn_G1.data(:,53), ...
%     5, windowData, toKeep);
% pp_ts_B1=poseFinder(pp_dataIn_B1.data(:,53), ...
%     5, windowData, toKeep);
% pp_ts_B2=poseFinder(pp_dataIn_B2.data(:,53), ...
%     5, windowData, toKeep);


cc=GetFeatures(headOn_dataIn_G1.data, headOn_ts_G1);
dd=GetFeatures(headOn_dataIn_B1.data, headOn_ts_B1);
ee=GetFeatures(headOn_dataIn_B2.data, headOn_ts_B2);

% cc=GetFeatures(ff_dataIn_G1.data, ff_ts_G1);
% dd=GetFeatures(ff_dataIn_G1.data, ff_ts_B1);
% ee=GetFeatures(ff_dataIn_B2.data, ff_ts_B2);

% cc=GetFeatures(pp_dataIn_G1.data, pp_ts_G1);
% dd=GetFeatures(pp_dataIn_G1.data, pp_ts_B1);
% ee=GetFeatures(pp_dataIn_B2.data, pp_ts_B2);


makePlots(cc, dd, ee);


% plot3(cc(1,:), cc(2,:), cc(3,:), 'go', ...
%     dd(1,:), dd(2,:), dd(3,:), 'ro', ...
%     ee(1,:), ee(2,:), ee(3,:), 'bo');


