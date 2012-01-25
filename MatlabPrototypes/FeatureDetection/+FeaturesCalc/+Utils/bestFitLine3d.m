function [ endpts ,residuals] = bestFitLine3d( X )
%UNTITLED5 Summary of this function goes here
%   X: The data in, with each row representing a data point

    [coeff,score,roots] = princomp(X);

    dirVect = coeff(:,1);

    [n,p] = size(X);
    meanX = mean(X,1);
    Xfit = repmat(meanX,n,1) + score(:,1:2)*coeff(:,1:2)';
    residuals = X - Xfit;
    
    extend=0;
    t = [min(score(:,1))-extend, max(score(:,1))+extend];
    endpts = [meanX + t(1)*dirVect'; meanX + t(2)*dirVect'];

end

