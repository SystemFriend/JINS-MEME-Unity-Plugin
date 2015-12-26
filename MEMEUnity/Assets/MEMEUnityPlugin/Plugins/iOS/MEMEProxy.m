#import "MEMEProxy.h"

@implementation MEMEProxy

static MEMEProxy *instance = NULL;

- (id) init:(NSString *)appClientId clientSecret:(NSString*)clientSecret {
    self = [super init];
    [MEMELib setAppClientId:appClientId clientSecret:clientSecret];
    [MEMELib sharedInstance].delegate = self;
    [[MEMELib sharedInstance] addObserver: self forKeyPath: @"centralManagerEnabled" options: NSKeyValueObservingOptionNew context:nil];
    return self;
}

- (void) observeValueForKeyPath:(NSString *)keyPath ofObject:(id)object change:(NSDictionary *)change context:(void *)context
{
    if ([keyPath isEqualToString: @"centralManagerEnabled"]){
        [self performSelector:@selector(scan) withObject:nil afterDelay:0.5f];
    }
}

- (void)scan {
    // Start Scanning
    MEMEStatus status = [[MEMELib sharedInstance] startScanningPeripherals];
    [self checkMEMEStatus: status];
}

// =============================================================================
#pragma mark
#pragma mark MEMELib Delegates

- (void) memePeripheralFound: (CBPeripheral *) peripheral withDeviceAddress:(NSString *)address
{
    NSLog(@"peripheral found %@", [peripheral.identifier UUIDString]);

    MEMEStatus status = [[MEMELib sharedInstance] connectPeripheral:peripheral];
    [self checkMEMEStatus: status];
}

- (void) memePeripheralConnected: (CBPeripheral *)peripheral
{
    NSLog(@"MEME Device Connected!");

}

- (void) memePeripheralDisconnected: (CBPeripheral *)peripheral
{
    NSLog(@"MEME Device Disconnected");
}

- (void) memeRealTimeModeDataReceived: (MEMERealTimeData *) data
{
    self.latestRealTimeData = data;
}

- (void) memeAppAuthorized:(MEMEStatus)status
{
    [self checkMEMEStatus: status];
}

#pragma mark UTILITY

- (void) checkMEMEStatus: (MEMEStatus) status
{
    if (status == MEME_ERROR_APP_AUTH){
        [[[UIAlertView alloc] initWithTitle: @"App Auth Failed" message: @"Invalid Application ID or Client Secret " delegate: nil cancelButtonTitle: nil otherButtonTitles: @"OK", nil] show];
    } else if (status == MEME_ERROR_SDK_AUTH){
        [[[UIAlertView alloc] initWithTitle: @"SDK Auth Failed" message: @"Invalid SDK. Please update to the latest SDK." delegate: nil cancelButtonTitle: nil otherButtonTitles: @"OK", nil] show];
    } else if (status == MEME_OK){
        NSLog(@"Status: MEME_OK");
    }
}

// =============================================================================
#pragma mark - UnityInterface

char* MakeStringCopy (const char* string) {
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

void MEMEStartSession(char *appClientId, char *appClientSecret) {
    if (instance != NULL) {
        return;
    }
    NSString *clientId = [NSString stringWithCString:appClientId encoding:NSUTF8StringEncoding];
    NSString *clientSecret = [NSString stringWithCString:appClientSecret encoding:NSUTF8StringEncoding];
    instance = [[MEMEProxy alloc] init:clientId clientSecret:clientSecret];
}

void MEMEEndSession() {
    [MEMELib sharedInstance].delegate = nil;
    instance = NULL;
}

char* MEMEGetSensorValues() {
    NSString *value = [NSString stringWithFormat:@"%f,%f,%f,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d",
                       instance.latestRealTimeData.pitch,
                       instance.latestRealTimeData.yaw,
                       instance.latestRealTimeData.roll,
                       instance.latestRealTimeData.eyeMoveUp,
                       instance.latestRealTimeData.eyeMoveDown,
                       instance.latestRealTimeData.eyeMoveLeft,
                       instance.latestRealTimeData.eyeMoveRight,
                       instance.latestRealTimeData.blinkSpeed,
                       instance.latestRealTimeData.blinkStrength,
                       instance.latestRealTimeData.isWalking,
                       instance.latestRealTimeData.accX,
                       instance.latestRealTimeData.accY,
                       instance.latestRealTimeData.accZ,
                       instance.latestRealTimeData.fitError,
                       instance.latestRealTimeData.powerLeft
                       ];
    NSLog(@"%@", value);
    return MakeStringCopy([value UTF8String]);
}

@end
