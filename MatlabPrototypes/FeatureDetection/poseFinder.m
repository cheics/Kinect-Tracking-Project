function [ peakLocations ] = poseFinder(dataIn, reps, dpw, np)
%poseFinder Summary of this function goes here
%   x:      The input data
%   reps:    Maximum number of reps to look for
%   dpw:    Desired around peak to sample poses
%   np:     Number on [0,1] to determine the percentage of 
%               extra window value to keep for processing
    
    yRightKneeData_ID=53;
    x=dataIn(:, yRightKneeData_ID);
    fc = 0.5; % Cut-off frequency (Hz)
    fs = 20; % Sampling rate (Hz)
    order = 3; % Filter order
    [B,A] = butter(order,2*fc/fs); % [0:pi] maps to [0:1] here
    y=filtfilt(B,A,x); % LPF

    [pks, loc]=findpeaks(y,...
        'MINPEAKHEIGHT', (min(y)+max(y))/4, ...
        'MINPEAKDISTANCE', round(size(y, 1)./(reps*1.5)), ...
        'NPEAKS', reps+1);
 
% % Peak debuging code
%     plot(1:size(x, 1), y, 1:size(x, 1), x, loc, pks, 'ro');
%     numPeaks=size(loc, 1);
%     for n = 1:numPeaks
%         debugPose(reshape(dataIn(loc(n), :), 3, 20)');
%         pause
    
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

