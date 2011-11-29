%%% Traning HELPER!!!
function [dataOut] = trainingHelper(poseType)
%% The training helper loads in all the data... then calculates features
%% this allows us to get a big data set


    basePath='C:\Users\cheics\Documents\Visual Studio 2010\Projects\';
    basePath2=strcat(basePath,'Beta2 Samples\Kinect-Tracking-Project\DataFiles\Experiment3\chopped');

    %% Select a type of data with POSETYPE
    dataType={'Front', '45', 'Profile'};

    defectType={'Good', 'BadBack', 'BadHip'};

    dataFiles_G1=dir(fullfile(basePath2,...
        strcat(char(dataType(poseType)), char(defectType(1)), '*.csv')));
    dataFiles_B1=dir(fullfile(basePath2,...
        strcat(char(dataType(poseType)), char(defectType(2)), '*.csv')));
    dataFiles_B2=dir(fullfile(basePath2,...
        strcat(char(dataType(poseType)), char(defectType(3)), '*.csv')));


    [features1, group1]=featureSplitter(basePath2, dataFiles_G1, 1);
    [features2, group2]=featureSplitter(basePath2, dataFiles_B1, 2);
    [features3, group3]=featureSplitter(basePath2, dataFiles_B2, 3);

    xx=vertcat(features1,features2,features3);
    yy=vertcat(group1,group2,group3);

    dataOut=horzcat(yy,xx);



% dd=zeros(0,3);
% for n=1:size(dataFiles_G1)
%    filePath=fullfile(basePath2,dataFiles_G1(n).name);
%    timeStepData=load(filePath);
%    poses=poseFinder(timeStepData, 5, 0, 1);
%    features=GetFeatures(timeStepData, poses)';
%    dd=vertcat(dd,features);
% end
% 
% goodFeatures=horzcat(ones(size(dd, 1), 1), dd);
% size(goodFeatures)



