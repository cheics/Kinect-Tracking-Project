function [ peakLocations ] = poseFinder(obj, jointLook, xyz, reps, dpw, np)
%poseFinder Summary of this function goes here
%   jointLook:   The joint ID to look at
%   reps:    Maximum number of reps to look for
%   dpw:    Desired around peak to sample poses
%   np:     Number on [0,1] to determine the percentage of 
%               extra window value to keep for processing
    
    rawDataIn=obj.skelData(:, obj.XYZ_IDS.(xyz), obj.jnts.(jointLook));
    fc = 0.5; % Cut-off frequency (Hz)
    fs = 20; % Sampling rate (Hz)
    order = 3; % Filter order
    [B,A] = butter(order,2*fc/fs); % [0:pi] maps to [0:1] here
    lpf_dataIn=filtfilt(B,A,rawDataIn); % LPF

    %'MINPEAKHEIGHT', (min(y)+max(y))/4, ...
    [pks, loc]=findpeaks(lpf_dataIn,...
        'MINPEAKDISTANCE', round(size(lpf_dataIn, 1)./(reps*1.5)), ...
        'NPEAKS', reps+1);
 
% Peak debuging code BEGIN
%     hold on;
%     plot(1:size(x, 1), x, 'k','LineWidth', 3);
%     plot(1:size(x, 1), y, 'b', 'LineWidth', 4);
%     plot(loc, pks, 'ro', 'MarkerSize', 10, 'LineWidth', 2);
%     hold off;
%     pause;
%     numPeaks=size(loc, 1);
%     for n = 1:numPeaks
%         debugPose(reshape(dataIn(loc(n), :), 3, 20)');
%         pause
%

    if dpw ~= 0
        %Making shaky sampling window
        samples=round(dpw*np);
        numPeaks=size(loc, 1);
        dstMat=zeros(numPeaks, 1+samples*2);
        for n = 1:numPeaks
            dd_neg=-dpw:-1;
            dd_pos=1:dpw;
            ss=sort(horzcat(randsample(dd_neg,samples),...
                    0, ...
                    randsample(dd_pos,samples)));
            dstMat(n, :)=ss;
        end

        peakColumns=loc*ones(1, 1+samples*2)+ dstMat;
        peakLocations=sort(reshape(peakColumns, ...
            1, size(peakColumns, 1)*size(peakColumns, 2)));
    else
        peakLocations=loc;
    end
    
    
end

