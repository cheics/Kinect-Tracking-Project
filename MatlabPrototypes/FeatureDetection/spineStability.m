function [ score ] = spineStability( hips, shoulderCentre, spine)
%UNTITLED3 Summary of this function goes here
%   Detailed explanation goes here


% Return variance in X and Z
x_set=[shoulderCentre(:,1), spine(:,1), hips(:,1)]-spine(:,1);
z_set=[shoulderCentre(:,3), spine(:,3), hips(:,3)]-spine(:,3);
score =std(x_set) + std(z_set);

end

