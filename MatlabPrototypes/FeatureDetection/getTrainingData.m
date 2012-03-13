function [tr_ft, tr_cl, tt_ft, tt_cl] = getTrainingData(exType, perTrain, varargin)
%% Help
%	OUTPUTS
%	tr_ft :	training feature vector
%	tr_cl :	training class vector
%	tt_ft :	test feature vector
%	tt_cl :	test class vector
%	INPUTS
%	exType		: excercise type string >> {squats}
%	perTrain	: percent training data
%	varargin	: contains dpw and np
%					dpw: desired size of sample window (0 is default)
%					np:  percent of samples to keep on [0-1]

%% Find the correct data files to load
existingTypes={'squats', 'legExt', 'legRaise', 'armRaise'};



if perTrain <0 || perTrain > 1
	err = MException('ResultChk:OutOfRange', ...
		'Training data percentage must be on [0,1], %f is invalid',...
		perTrain ...
	);
    throw(err)
end

baseDir='../../DataFiles/FinalTrainingData/';
[dataFilePaths,peakDetectJoint,peakDetectDim,detectPeakHigh]=getExcercise_params(exType);

%% Get the paths
outPaths=cell(0,1);
for i=1:length(dataFilePaths)
	pp=dataFilePaths{i};
	dataFileNames=ls(strcat(baseDir, pp, '/*.mat'));
	numberFiles=size(dataFileNames,1);
	for j=1:numberFiles
		outPaths=[outPaths; ...
			strcat(baseDir,pp, '/', dataFileNames(j,:))];
	end
end


%% Load the data and output feature vectors
trainingData=struct();
for i = 1:length(outPaths)
	cc=FactoryKinectData(outPaths{i});
	
	% Peak detection details per excercise
	cc.peakDetectJoint=peakDetectJoint;
	cc.joint_xyz=peakDetectDim;
	cc.findMax=detectPeakHigh;
	
	if numel(varargin) == 2
		cc.dpw=varargin{1};
		cc.np=varargin{2};
	end
			
	cc.GetFeatures();
	if i==1
		trainingData.features=cc.featureResults.featureVector;
		trainingData.classes=cc.featureResults.classVector;
	else
		trainingData.features=vertcat(trainingData.features, cc.featureResults.featureVector);
		trainingData.classes=vertcat(trainingData.classes, cc.featureResults.classVector);
	end
end


%% Split into training and testing sets
% randomize order
numberEntries=size(trainingData.classes,1);
ordering = randperm(numberEntries);
trainingData.classes=trainingData.classes(ordering, :);
trainingData.features=trainingData.features(ordering, :);

% split sets
if perTrain==1
	tr_ft=trainingData.features;
	tr_cl=trainingData.classes;
else
	testTrainDivide=round(numberEntries*perTrain);
	tr_ft=trainingData.features(1:testTrainDivide,:);
	tr_cl=trainingData.classes(1:testTrainDivide,:);
	tt_ft=trainingData.features(testTrainDivide:end,:);
	tt_cl=trainingData.classes(testTrainDivide:end,:);
end



end