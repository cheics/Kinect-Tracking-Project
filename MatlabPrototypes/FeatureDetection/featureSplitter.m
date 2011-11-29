function [ outFeatures, types] = featureSplitter(path, dataFiles, type )


    dd=zeros(0,3);
    for n=1:size(dataFiles)
       filePath=fullfile(path,dataFiles(n).name);
       timeStepData=load(filePath);
       poses=poseFinder(timeStepData, 5, 0, 1);
       features=GetFeatures(timeStepData, poses)';
       dd=vertcat(dd,features);
    end

    outFeatures=dd;
    types=type*ones(size(dd, 1), 1);
end

