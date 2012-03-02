function [tr_ft, tr_cl, tt_ft, tt_cl] = getTrainingData(exType, perTrain)
%% Help
%	OUTPUTS
%	tr_ft :	training feature vector
%	tr_cl :	training class vector
%	tt_ft :	test feature vector
%	tt_cl :	test class vector
%	INPUTS
%	exType		: excercise type string >> {squats}
%	perTrain	: percent training data

%% Find the correct data files to load
existingTypes={'squats'};


if perTrain <0 || perTrain > 1
	err = MException('ResultChk:OutOfRange', ...
		'Training data percentage must be on [0,1], %f is invalid',...
		perTrain ...
	);
    throw(err)
end

if strcmp(exType, 'squats')
	dataFileNames=ls('../../DataFiles/CritComp_squats1/*.mat');
	peakDetectJoint='HIP_C';
	peakDetectDim='Y';
	detectPeakHigh=false;
else
	err = MException('ResultChk:BadInput', ...
		'Excercise type %s is not valid, must be one of {%s}',...
		dataFile, ...
		strtrim(sprintf('%s ', existingTypes{:})) ...
	);
    throw(err)
end

%% Load the data and output feature vectors
numberFiles=size(dataFileNames); numberFiles=numberFiles(1);
dataFilePath=horzcat(repmat('../../DataFiles/CritComp_squats1/', numberFiles, 1),dataFileNames);

trainingData=struct();
for i = 1:numberFiles
	fileName=dataFilePath(i,:);
	cc=FactoryKinectData(fileName);
	
	% Peak detection details per excercise
	cc.peakDetectJoint=peakDetectJoint;
	cc.joint_xyz=peakDetectDim;
	cc.findMax=detectPeakHigh;
			
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