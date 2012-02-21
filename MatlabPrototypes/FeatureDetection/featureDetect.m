%clc;

% get data files
dataFileNames=ls('../../DataFiles/CritComp_squats1/*.mat');

numberFiles=size(dataFileNames); numberFiles=numberFiles(1);
dataFilePath=horzcat(repmat('../../DataFiles/CritComp_squats1/', numberFiles, 1),dataFileNames);

trainingData=struct();
trainingData.classes=zeros(0,3);
trainingData.features=zeros(0,15);
for i = 1:numberFiles
	fileName=dataFilePath(i,:);
	cc=FactoryKinectData(fileName);
	cc.GetFeatures();
	trainingData.classes=vertcat(trainingData.classes, cc.featureResults.classVector);
	trainingData.features=vertcat(trainingData.features, cc.featureResults.featureVector);
end


% Need to prealocate feature and other vectors



% headOn_dataIn_G1=importdata(strcat(basePath2,'headOnGood.csv'));
% headOn_dataIn_B1=importdata(strcat(basePath2,'headOnBadBack.csv'));
% headOn_dataIn_B2=importdata(strcat(basePath2,'headOnBadHip.csv'));
% 
% ff_dataIn_G1=importdata(strcat(basePath2,'45Good.csv'));
% ff_dataIn_B1=importdata(strcat(basePath2,'45BadBack.csv'));
% ff_dataIn_B2=importdata(strcat(basePath2,'45BadHip.csv'));
% 
% pp_dataIn_G1=importdata(strcat(basePath2,'profileGood.csv'));
% pp_dataIn_B1=importdata(strcat(basePath2,'profileBadBack.csv'));
% pp_dataIn_B2=importdata(strcat(basePath2,'profileBadHip.csv'));



% headOn_ts_G1=poseFinder(headOn_dataIn_G1.data, ...
%     reps_in_data, windowData, toKeep);
% headOn_ts_B1=poseFinder(headOn_dataIn_B1.data, ...
%     reps_in_data, windowData, toKeep);
% headOn_ts_B2=poseFinder(headOn_dataIn_B2.data, ...
%     reps_in_data, windowData, toKeep);
% 
% ff_ts_G1=poseFinder(ff_dataIn_G1.data, ...
%     reps_in_data, windowData, toKeep);
% ff_ts_B1=poseFinder(ff_dataIn_B1.data, ...
%     reps_in_data, windowData, toKeep);
% ff_ts_B2=poseFinder(ff_dataIn_B2.data, ...
%     reps_in_data, windowData, toKeep);
% 
% pp_ts_G1=poseFinder(pp_dataIn_G1.data, ...
%     reps_in_data, windowData, toKeep);
% pp_ts_B1=poseFinder(pp_dataIn_B1.data, ...
%     reps_in_data, windowData, toKeep);
% pp_ts_B2=poseFinder(pp_dataIn_B2.data, ...
%     reps_in_data, windowData, toKeep);
% 
% 
% cc1=GetFeatures(headOn_dataIn_G1.data, headOn_ts_G1);
% dd1=GetFeatures(headOn_dataIn_B1.data, headOn_ts_B1);
% ee1=GetFeatures(headOn_dataIn_B2.data, headOn_ts_B2);

% cc2=GetFeatures(ff_dataIn_G1.data, ff_ts_G1);
% dd2=GetFeatures(ff_dataIn_G1.data, ff_ts_B1);
% ee2=GetFeatures(ff_dataIn_B2.data, ff_ts_B2);
% 
% cc3=GetFeatures(pp_dataIn_G1.data, pp_ts_G1);
% dd3=GetFeatures(pp_dataIn_G1.data, pp_ts_B1);
% ee3=GetFeatures(pp_dataIn_B2.data, pp_ts_B2);


%%%
%%% 1 - frontal
%%% 2 - 3/4 view
%%% 3 - profile
%makePlots(cc1, dd1, ee1);
%score = clustering2(cc, dd, ee);

% cc=cc1;
% dd=dd1;
% ee=ee1;
% 
% plot( ones(size(cc(1,:))), cc(1,:), 'go',...
%     2.*ones(size(dd(1,:))), dd(1,:), 'ro',...
%     3.*ones(size(ee(1,:))), ee(1,:), 'bo'...
%     );
% 
% range=horzcat(cc(1,:), dd(1,:), ee(1,:));
% ll=min(range)-(max(range)-min(range))*0.1;
% ul=max(range)+(max(range)-min(range))*0.1;
% xlim([0, 4]);
% ylim([ll ul]);
% xlabel('Pose classification');
% ylabel('Spine stability score');
%makePlots(cc, dd, ee);
% plot3(cc(1,:), cc(2,:), cc(3,:), 'go', ...
%     dd(1,:), dd(2,:), dd(3,:), 'ro', ...
%     ee(1,:), ee(2,:), ee(3,:), 'bo');


