labels  = poseFeatures.classifierData;
data = poseFeatures.featureData;

stringlabel = labels(:,1);

datasize = size (data);

list = 1 : datasize(2);
matrix = zeros (datasize(1),datasize(2),2);

%for i = 1 : 3
   
%    cc = labels(:,i);
    
    for j = 1 : datasize(1)
    
        for k = 1 : datasize(2)
            
            temp = list;
            temp(k) = 0;
            temp = nonzeros (temp);
            
            data1 = data(:,temp);
            x = data(j,temp);

            Gdata = data1(find (stringlabel == 0),:);            
            u1 = mean(Gdata);
            covar1 = cov(Gdata);
            
            Gdata = data1(find (stringlabel == 1),:);            
            u2 = mean(Gdata);
            covar2 = cov(Gdata);
            
            Gdata = data1(find (stringlabel == 2),:);            
            u3 = mean(Gdata);
            covar3 = cov(Gdata);
            
            [matrix(j,k,1),matrix(j,k,2)] = GEDClassifier(x,u1,u2,u3,covar1,covar2,covar3);

        end
    
    end
    
%end
