%load('FeatureDetection/trainingData_skeletonKin.mat');
load('FeatureDetection/trainingData_trig.mat');
labels  = trainingData.classes;
data = trainingData.features;

% Automatically sizes data
datasize = size(data);
dataHere=[];
for i=1:datasize(2)
	if length(unique(data(:,i))) > 2
		dataHere=[dataHere, i];
	end
end
data = data(:,dataHere);
datasize = size(data);


ICD = zeros (datasize(2),3,3);

for i = 1 : 3
    
    stringlabel = labels(:,i);
    
    for j = 1 : datasize(2)
        
        choose1 = find (stringlabel == 0);
        Gdata = data(choose1,j);            
        u1 = mean(Gdata);
        covar1 = cov(Gdata);
        
        choose2 = find (stringlabel == 1);
        Gdata = data(choose2,j);          
        u2 = mean(Gdata);
        covar2 = cov(Gdata);

        choose3 = find (stringlabel == 2);
        Gdata = data(choose3,j);          
        u3 = mean(Gdata);
        covar3 = cov(Gdata);

        Icovar1 = (length(choose1) * covar1 / datasize(1)) +  (length(choose2) * covar2 / datasize(1));
        Icovar2 = (length(choose1) * covar1 / datasize(1)) +  (length(choose3) * covar3 / datasize(1));
        Icovar3 = (length(choose2) * covar2 / datasize(1)) +  (length(choose3) * covar3 / datasize(1));
        
        ICD(j,1,i) = (u1 - u2) * (inv(Icovar1)) * transpose(u1 - u2);
        ICD(j,2,i) = (u1 - u3) * (inv(Icovar2)) * transpose(u1 - u3);
        ICD(j,3,i) = (u2 - u3) * (inv(Icovar3)) * transpose(u2 - u3);
        
    end
    
end 

figure();
critComps=cell(3,1);
critComps{1}='SquatDepth';
critComps{2}='StraightBack';
critComps{3}='Balance';

for g = 1 : 3
    vizualICD = zeros(datasize(2)*3,2);
    vizualICD (:,2) = nonzeros(ICD(:,:,g));
    k = 1;
	for i = 1:3
		for j = 1 : datasize(2)
			vizualICD(k,1) = j;
			k = k + 1;
		end
	end
	subplot(3,1,g);
	plot(vizualICD(:,1),vizualICD(:,2),'*b');
	title(critComps{g});
end