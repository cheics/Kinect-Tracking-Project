function [ spineScore ] = spineStability2(allDataVector)
%UNTITLED3 Summary of this function goes here
%   Detailed explanation goes here

[head,shoulderCentre, spine]=deal(4,3,2);
[hipLeft, hipCentre, hipRight]=deal(13,1, 17);
[ankleLeft, kneeLeft]=deal(15,14);

hips=vertcat(...
    allDataVector(hipLeft, :), ...
    allDataVector(hipCentre, :), ...
    allDataVector(hipRight, :)...
);

spine=vertcat(...
    allDataVector(head, :), ...
    allDataVector(shoulderCentre, :), ...
    allDataVector(spine, :),...
    allDataVector(hipCentre, :)...
);

leftLeg=vertcat(...
    allDataVector(ankleLeft, :), ...
    allDataVector(kneeLeft, :), ...
    allDataVector(hipLeft, :)...
);

endpts1=bestFitLine3d(hips);
[endpts2, spineResid]=bestFitLine3d(spine);
[endpts3]=bestFitLine3d(leftLeg);

v1=endpts3(1, :)-endpts3(2, :);
v2=endpts2(2, :)-endpts2(1, :);


% plot3(endpts3(:, 1), endpts3(:, 3), endpts3(:, 2), 'k-x',...
%      endpts2(:, 1), endpts2(:, 3), endpts2(:, 2), 'b-o',...
%     endpts1(:, 1), endpts1(:, 3), endpts1(:, 2), 'r-o');
% axis equal; 
% pause


spineScore=acos(dot(v1, v2)./(norm(v1)*norm(v2)))*(180/pi);

% debugPose(allDataVector);
% hold on;
% plot3(endpts3(:, 1), endpts3(:, 3), endpts3(:, 2), 'k-x');
% plot3(endpts2(:, 1), endpts2(:, 3), endpts2(:, 2), 'k-x');
% hold off;
% spineScore
% pause


end


