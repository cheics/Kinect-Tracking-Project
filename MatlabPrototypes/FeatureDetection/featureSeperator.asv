function [ output_args ] = featureSeperator( dataIn )
%UNTITLED5 Summary of this function goes here
    %dataIn:    Matrix of data sets, with the first dimension containing
    %           sepeartoin of data type
    
    reps_in_data=5;
    windowData=0;
    toKeep=1;
    
    kneeID=53;
    
    timestamps=zeros(size(dataIn,1), 
    for n = 1:size(dataIn,1)
        excercise=dataIn(n, :, :);
        timeStamps=poseFinder(excercise(:,kneeID), ...
            reps_in_data, windowData, toKeep);
    end


% ff_dataIn_G1=importdata(strcat(basePath2,'45Good.csv'));
% ff_dataIn_B1=importdata(strcat(basePath2,'45BadBack.csv'));
% ff_dataIn_B2=importdata(strcat(basePath2,'45BadHip.csv'));


cc=GetFeatures(headOn_dataIn_G1.data, headOn_ts_G1);
dd=GetFeatures(headOn_dataIn_G1.data, headOn_ts_B1);
ee=GetFeatures(headOn_dataIn_B2.data, headOn_ts_B2);


makePlots(cc, dd, ee);


end

